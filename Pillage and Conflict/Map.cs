using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
   public  class Map
    {
        public int Width;
        public int Height;
        public List<List<Tile>> Tiles = new List<List<Tile>>();

        public void Add(List<Tile> items)
        {
            Tiles.Add(items);
        }
    }
}
