using BatailleNavaleServer.Models.Enums;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatailleNavaleServer.Models
{
    public class Game
    {


        public IEnumerable<int> GetCellsAround(Partie p, int x, int y)
        {

            for (var i = -1; i <= 1; i++)
                for (var j = -1; j <= 1; j++)
                {
                    var v = p.getCell(x + i, y + j);
                    if (v != null) yield return v.Value;
                }
        }

        public int ValueFromCellState(ECellState state)
        {
            var value = 0;

            switch (state)
            {
                case ECellState.Vide:
                    value = 0;
                    break;
                case ECellState.Occupe:
                    value = 1;
                    break;
                case ECellState.Plouf:
                    value = 2;
                    break;
                case ECellState.Touche:
                    value = 3;
                    break;
                case ECellState.Coule:
                    value = 4;
                    break;
            }

            return value;
        }

        public Partie Initialise(string name, EDifficulty difficulte)
        {
            var nbMissiles = 0;
            switch (difficulte)
            {
                case EDifficulty.Facile:
                    nbMissiles = 10 * 10;
                    break;
                case EDifficulty.Moyen:
                    nbMissiles = 8 * 8;
                    break;
                case EDifficulty.Difficile:
                    nbMissiles = 7 * 7;

                    break;
                case EDifficulty.Hardcore:
                    nbMissiles = 6 * 6;
                    break;
            }
            var partie = new Partie()

            {
                Name = name,
                NbMissiles = nbMissiles

            };

            for (var i = 0; i < 1; i++) PutBoat(partie, 5);
            for (var i = 0; i < 2; i++) PutBoat(partie, 4);
            for (var i = 0; i < 3; i++) PutBoat(partie, 3);
            for (var i = 0; i < 4; i++) PutBoat(partie, 2);
            return partie;
        }

        private bool PutBoat(Partie partie, int boatsize)
        {
            
                var random = new Random();
                var fit = true;

                for (var tentatives = 0; tentatives < 1000; tentatives++)
                {
                    var direction = random.Next(4);
                    var dx = (direction < 2 ? 1 : 0) * (direction % 2 == 0 ? 1 : -1);
                    var dy = (direction < 2 ? 0 : 1) * (direction % 2 == 0 ? 1 : -1);

                    var x = random.Next(Partie.DIMENSION);
                    var y = random.Next(Partie.DIMENSION);


                // On teste s'il y a de la place pour le bateau à l'emplacement choisi
                    for (var i = 0; i < boatsize; i++) 
                    {
                        var posx = x + dx * i;
                        var posy = y + dy * i;

                        if (partie.getCell(posx, posy) != ValueFromCellState(ECellState.Vide) || GetCellsAround(partie, posx, posy).Any(c => c != ValueFromCellState(ECellState.Vide)))
                        {
                            fit = false; // il n'y a pas la place pour le bateau
                            break;
                        }
                    }

                // S'il y a la place, on installe le bateau
                    if (fit) 
                    {
                        for (var i = 0; i < boatsize; i++)
                        {
                            var posx = x + dx * i;
                            var posy = y + dy * i;
                            partie.setCell(posx, posy, ValueFromCellState(ECellState.Occupe));
                        }
                        return true;
                    }

                    
                }

                return false;
            
        }

            // Retourne le nombre de cases occupées restantes
            public int Jouer(Partie p, int x, int y)
            {
                if (p.getCell(x, y) == ValueFromCellState(ECellState.Vide))
                { p.setCell(x, y, ValueFromCellState(ECellState.Plouf)); }

                if (p.getCell(x, y) == ValueFromCellState(ECellState.Occupe))
                { p.setCell(x, y, ValueFromCellState(ECellState.Touche)); }

                var reste = p.Grid.Where(c => c == ValueFromCellState(ECellState.Occupe)).Count();

                return reste;
            }
        }

    }
    

