﻿using Microsoft.Xna.Framework.Graphics;

namespace Pillage_and_Conflict.Classes
{
    public class Tile
    {
        public Texture2D Texture;
        public int id;
        public float layer;
        public int Width = 20;
        public int Height = 20;
        public bool passable;

        public Tile(int Value, float Layer, bool Passable)
        {
            id = Value;
            Texture = PillageandConflict.Textures[Value];
            layer = Layer;
            passable = Passable;
        }
    }
}