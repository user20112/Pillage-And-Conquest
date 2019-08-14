using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pillage_and_Conflict.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Tiles
    {
        public List<Tile> tiles;
        float TopLayer = .5f;
        float BottomLayer = .4f;
        public Tiles(int Value)
        {
            tiles = new List<Tile>();
            tiles.Add(new Tile(Value, TopLayer));
        }
        public void Layer(int value)
        {
            TopLayer += .1f;
            tiles.Add(new Tile(value, TopLayer));
        }
        public void addtobottom(int value)
        {
            BottomLayer -= .1f;
            tiles.Add(new Tile(value, BottomLayer));
        }
    }
}
