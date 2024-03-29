﻿using Pillage_and_Conflict.Classes;
using System.Collections.Generic;

namespace Pillage_and_Conflict
{
    public class Tiles
    {
        public List<Tile> tiles;
        private float TopLayer = .5f;
        private float BottomLayer = .4f;

        public Tiles(int Value, bool passable)
        {
            tiles = new List<Tile>();
            tiles.Add(new Tile(Value, TopLayer, passable));
        }

        public void Layer(int value, bool passable)
        {
            TopLayer += .1f;
            tiles.Add(new Tile(value, TopLayer, passable));
        }

        public void addtobottom(int value, bool passable)
        {
            BottomLayer -= .1f;
            tiles.Add(new Tile(value, BottomLayer, passable));
        }

        public bool ContainsID(int id)
        {
            foreach (Tile tile in tiles)
                if (tile.id == id)
                    return true;
            return false;
        }

        public void remove(int id)
        {
            Tile toberemoved = new Tile(1, 0, false);
            foreach (Tile tile in tiles)
            {
                if (tile.id == id)
                {
                    toberemoved = tile;
                }
            }
            tiles.Remove(toberemoved);
        }
    }
}