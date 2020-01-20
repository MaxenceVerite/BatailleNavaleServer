using BatailleNavaleServer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Partie
    {

        public Partie()
        {

            Grid = new byte[DIMENSION * DIMENSION];
            Date = DateTime.Now;
        }

        public static int DIMENSION = 12;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
     
        public byte[] Grid { get; set; } 

        public DateTime Date { get; set; }

        public int NbMissiles { get; set; }

        public  void setCell(int x, int y, int value)
        {
            
            Grid[y * x] = (byte)value;
        }
        public  int? getCell(int x,int y)
        {
            if (x < 0 || x >= DIMENSION) return null;
            if (y < 0 || y >= DIMENSION) return null;
                var index = y * DIMENSION + x;
            return Grid[index];
        }


       
    }
}
