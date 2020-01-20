using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BatailleNavaleServer.Models;
using Server.Models;
using BatailleNavaleServer.Models.Enums;

namespace BatailleNavaleServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartiesController : ControllerBase
    {
        private readonly BatailleContext _context;
        private Game game;

        public PartiesController(BatailleContext context, Game game)
        {
            _context = context;
            this.game = game;
        }

        // GET: api/Parties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partie>>> GetParties()
        {
            return await _context.Parties.ToListAsync();
        }

        [HttpGet("")]
        public object Get()
        {
            return this._context.Parties.Select(c => new {
                id =c.Id,
                name = c.Name,
                nbMissiles = c.NbMissiles,
                date = c.Date,
                grid = c.Grid.Select(c => (int)c).ToArray()
            }).ToList();
        }

        [HttpGet("Play/{id:Guid}")]
        public object Play(Guid id, int x, int y)
        {
            var partie = this._context.Parties.Find(id);

            var casesOccupeesRestantes = this.game.Jouer(partie, x, y);

            var grille = partie.Grid.Select(c => (int)c).ToArray();
            var etatCase = partie.getCell(x, y);
            this._context.Update(partie);
            this._context.SaveChanges();

            return new { reste = casesOccupeesRestantes, grid = grille, etat = etatCase };
        }


        [HttpGet("Create/{name}/{difficulte}")]
        public object Create(string name, string difficulte)
        {
            EDifficulty dif= EDifficulty.Facile;

            // mapping difficulte
            switch (difficulte)
            {
                case "facile": dif = EDifficulty.Facile; break;
                case "moyen": dif = EDifficulty.Moyen; break;
                case "difficile": dif = EDifficulty.Difficile; break;
                case "hardcore": dif = EDifficulty.Hardcore; break;
            }

            var partie = game.Initialise(name,dif);

            
            this._context.Parties.Add(partie);
            this._context.SaveChanges();

            return new
            {
                id = partie.Id,
                name = partie.Name,
                nbMissiles = partie.NbMissiles,
                date = partie.Date,
                grid = partie.Grid.Select(c => (int)c).ToArray()
            };
        }


        [HttpGet("GetPartie/{id:Guid}")]
        public object GetPartie(Guid id)
        {
            var c = this._context.Parties.Find(id);
            return new
            {
                id = c.Id,
                name = c.Name,
                nbMissiles = c.NbMissiles,
                date= c.Date,
                grid = c.Grid.Select(c => (int)c).ToArray()
            };
        }



        // DELETE: api/Parties/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Partie>> DeletePartie(Guid id)
        {
            var partie = await _context.Parties.FindAsync(id);
            if (partie == null)
            {
                return NotFound();
            }

            _context.Parties.Remove(partie);
            await _context.SaveChangesAsync();

            return partie;
        }

        private bool PartieExists(Guid id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }


        
    }
}
