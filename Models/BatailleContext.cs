using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatailleNavaleServer.Models
{
    public class BatailleContext : DbContext
    {

        public DbSet<Partie> Parties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Partie>().HasKey(c => c.Id);

            var partie1 = new Partie()
            {
                Name = "Combat à la surface de l'eau entre navigateurs",
                NbMissiles = 20
            };

            modelBuilder.Entity<Partie>().HasData(partie1);




        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Trusted_Connection=True;Database=BatailleNavaleDB");



        }
    }
}
