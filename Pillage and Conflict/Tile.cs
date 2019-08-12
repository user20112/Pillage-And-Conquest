using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Tile
    {
        public Texture2D Texture;
        public int id;
        ContentManager Content;
        public int Width = 20;
        public int Height=20;

        public Tile(int Value, ContentManager content)
        {
            id = Value;
            Content = content;
            Texture = PillageandConflict.Textures[id];
        }
    }
}
