using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Tiles
    {
        public List<Texture2D> Textures;
        public List<int> id;
        public int Width = 20;
        public int Height = 20;

        public Tiles(int Value)
        {
            List<Texture2D> Textures = new List<Texture2D>();
            id = new List<int>();
            id.Add(Value);
            Textures.Add(PillageandConflict.Textures[Value]);
        }
        public void Layer(int value)
        {
            id.Add(value);
            Textures.Add(PillageandConflict.Textures[value]);
        }
        public void addtobottom(int value)
        {
            id.Insert(0, value);
            Textures.Insert(0, PillageandConflict.Textures[value]);
        }
    }
}
