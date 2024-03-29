﻿using Pillage_and_Conflict.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pillage_and_Conflict
{
    public class Map
    {
        public int Width;
        public int Height;
        public List<List<Tiles>> Tiles = new List<List<Tiles>>();
        public List<List<Tiles>> Structures = new List<List<Tiles>>();
        public List<Projectile> Projectiles;
        public List<Mob> Mobs=new List<Mob>();

        public void Add(List<Tiles> items)
        {
            Tiles.Add(items);
        }

        static public Map generatemap(int Height, int Width)
        {
            Map Map = new Map();
            Map.Projectiles = new List<Projectile>();
            for (int y = 0; y <= Height; y++)
            {
                List<Tiles> Row = new List<Tiles>();
                for (int x = 0; x <= Width; x++)
                {
                    Row.Add(new Tiles(280, true));
                }
                Map.Tiles.Add(Row);
            }
            if (Height > 60 && Width > 60)
            {
                Random rand = new Random();
                for (int x = 0; x < Height / 80; x++)
                {
                    Map.GenerateDirtpatch(rand.Next(50, Height - 50), rand.Next(50, Width - 50), rand.Next(40, 50));
                }
                for (int x = 0; x < Height / 5; x++)
                {
                    Map.GenerateSandpatch(rand.Next(50, Height - 50), rand.Next(50, Width - 50), rand.Next(40, 50));
                }
                for (int x = 0; x < Height / 20; x++)
                {
                    // Map.Layer(rand.Next(20, Height - 20), rand.Next(20, Width - 20), Map.LoadMap("Structures/SmallPond.TXT"));

                }
               // Map.Layer(rand.Next(20, Height - 20), rand.Next(20, Width - 20), Map.LoadMap("Structures/SmallPond.TXT"));
                Map.Layer(80, 80, Map.LoadMap("Structures/SmallPond.TXT"));
                //for (int x = 0; x < Height / 40; x++)
                //{
                //    Map.Layer(rand.Next(40, Height - 40), rand.Next(40, Width - 40), Map.LoadMap("Structures/LargePond.TXT"));
                //}
            }
            Map.Width = Width;
            Map.Height = Height;
            return Map;
        }

        public void GeneratePond(int positionx, int positiony, int width)
        {
            bool FirstWater = true;
            bool LastWater = true;
            int ToCoverLeft = (int)Math.Ceiling((double)width / 3);
            int ToCoverRight = (int)width / 3 + width % 3;
            int NumberOfRowsToCoverInTop = ((int)width / 3) - 1;
            int NumberOfRowsToCoverInBottom = ((int)width / 3) - 1;
            for (int x = positionx; x <= width + positionx; x++)
            {
                if (x <= (width / 3) * 2 + positionx && x > (width / 3) + positionx)//Second third
                {
                    if (FirstWater)
                    {
                        Tiles[x][positiony].Layer(294, false);
                        FirstWater = false;
                    }
                    else
                        if (x == width / 3 * 2 + positionx)//last water
                        Tiles[x][positiony].Layer(296, false);
                    else
                        Tiles[x][positiony].Layer(295, false);
                }
                if (x <= (width / 3) * 2 + positionx && x > (width / 3) + positionx)//Second third
                {
                    if (LastWater)
                    {
                        Tiles[x][positiony + width].Layer(446, false);
                        LastWater = false;
                    }
                    else
                        if (x == width / 3 * 2 + positionx)//last water
                        Tiles[x][positiony + width].Layer(448, false);
                    else
                        Tiles[x][positiony + width].Layer(447, false);
                }
            }
            int NumberOfTimesTopLeft = 0;
            int NumberOfTimesTopRight = 0;
            for (int y = width + positiony; y >= positiony; y--)
            {
                if (y >= positiony + (width / 3) && y <= positiony + (width / 3) * 2)
                {
                    for (int x = positionx; x <= width + positionx; x++)
                    {
                        if (x == positionx)
                        {
                            Tiles[x][y].Layer(406, false);
                        }
                        else
                        {
                            if (x == width + positionx)
                            {
                                Tiles[x][y].Layer(408, false);
                            }
                            else
                            {
                                Tiles[x][y].Layer(407, false);
                            }
                        }
                    }
                }
                else
                {
                    if (y == positiony || y == positiony + width) { }
                    else
                    {
                        int NumberDonel = 0;
                        int NumberDoner = 0;
                        if (y <= positiony + (width / 2))//if above the midpoint
                        {
                            int SpacesBeforePondRow = (ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft + 1));
                            NumberDonel = SpacesBeforePondRow - 1;
                            for (int x = positionx + 1 + SpacesBeforePondRow; x > positionx + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft); x--)
                            {
                                Tiles[x + 1][y].Layer(391, false);
                                NumberDonel++;
                            }
                            if (NumberOfTimesTopLeft < NumberOfRowsToCoverInTop - 1)
                            {
                                Tiles[positionx + 1 + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft)][y].Layer(550, false);
                                Tiles[positionx + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft)][y].Layer(318, false);
                                NumberDonel++;
                                NumberDonel++;
                            }
                            else
                            {
                                int temp = 0;
                                while (!Tiles[positionx + 1 + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft) + temp][y - 1].ContainsID(294))
                                {
                                    Tiles[positionx + 1 + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft) + temp][y].Layer(319, false);
                                    Tiles[positionx + 1 + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft) + temp][y].remove(391);
                                    temp++;
                                }
                                NumberDonel++;
                                Tiles[positionx + 1 + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft) + temp][y].Layer(550, false);
                                NumberDonel++;
                                Tiles[positionx + ToCoverLeft / NumberOfRowsToCoverInTop * (NumberOfTimesTopLeft)][y].Layer(318, false);
                                NumberDonel++;
                            }
                            NumberOfTimesTopLeft++;
                            //right side next
                            SpacesBeforePondRow = (ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight + 1));
                            NumberDoner = SpacesBeforePondRow - 1;
                            for (int x = positionx + 1 + SpacesBeforePondRow; x > positionx + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight); x--)
                            {
                                Tiles[x + 1][y].Layer(391, false);
                                NumberDoner++;
                            }
                            if (NumberOfTimesTopRight < NumberOfRowsToCoverInTop - 1)
                            {
                                Tiles[positionx + 1 + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight)][y].Layer(550, false);
                                Tiles[positionx + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight)][y].Layer(318, false);
                                NumberDoner++;
                                NumberDoner++;
                            }
                            else
                            {
                                int temp = 0;
                                while (!Tiles[positionx + 1 + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight) + temp][y - 1].ContainsID(294))
                                {
                                    Tiles[positionx + 1 + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight) + temp][y].Layer(319, false);
                                    Tiles[positionx + 1 + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight) + temp][y].remove(391);
                                    temp++;
                                }
                                NumberDoner++;
                                Tiles[positionx + 1 + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight) + temp][y].Layer(550, false);
                                NumberDoner++;
                                Tiles[positionx + ToCoverRight / NumberOfRowsToCoverInTop * (NumberOfTimesTopRight)][y].Layer(318, false);
                                NumberDoner++;
                            }
                            NumberOfTimesTopRight++;
                        }
                        else//if below the midpoint
                        {
                        }
                        NumberDonel--;
                        while (NumberDonel < Math.Ceiling((double)width / 2))
                        {
                            Tiles[positionx + NumberDonel][y].Layer(391, false);
                            NumberDonel++;
                        }
                        NumberDoner--;
                        while (NumberDoner < Math.Ceiling((double)width / 2))
                        {
                            Tiles[positionx + NumberDoner][y].Layer(391, false);
                            NumberDoner++;
                        }
                    }
                }
            }
        }

        public void GenerateDirtpatch(int position, int positiony, int width)
        {
        }

        public void GenerateSandpatch(int position, int positiony, int width)
        {
        }

        public void Layer(int position, int positiony, Map map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    foreach (Tile tile in map.Tiles[y][x].tiles)
                    {
                        if (tile.id != 0)
                            Tiles[y + positiony][x + position].Layer(tile.id, tile.passable);
                    }
                }
            }
        }

        public static Map LoadMap(string MapName)
        {
            List<Tiles> Row = null;
            Map Map = new Map();
            string items = File.ReadAllText(Directory.GetCurrentDirectory() + "/" + MapName);
            string[] Rows = items.Split('/');
            for (int CurrentRow = 0; CurrentRow < Rows.Length - 1; CurrentRow++)
            {
                Row = new List<Tiles>();
                string[] Tiles = Rows[CurrentRow].Split(']');
                for (int CurrentTile = 0; CurrentTile < Tiles.Length - 1; CurrentTile++)
                {
                    bool first = true;
                    string[] Values = Tiles[CurrentTile].Split(',');
                    for (int CurrentLayer = 0; CurrentLayer < Values.Length; CurrentLayer += 2)
                    {
                        if (first)
                        {
                            Row.Add(new Tiles(Convert.ToInt32(Values[CurrentLayer]), Convert.ToInt32(Values[CurrentLayer + 1]) == 1));
                            first = false;
                        }
                        else
                            Row[CurrentTile].Layer(Convert.ToInt32(Values[CurrentLayer]), Convert.ToInt32(Values[CurrentLayer + 1]) == 1);
                    }
                }
                Map.Tiles.Add(Row);
            }
            //its reverse laoded so unreverse the array

            Map.Width = Row.Count;
            Map.Height = Map.Tiles.Count;
            Map ResultMap = new Map();
            for (int x = 0; x < Map.Width; x++)
            {
                ResultMap.Tiles.Add(new List<Tiles>());
            }
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    bool first = true;
                    foreach (Tile tile in Map.Tiles[x][y].tiles)
                    {
                        if (first)
                            ResultMap.Tiles[y].Add(new Tiles(tile.id, tile.passable));
                        else
                            ResultMap.Tiles[y][x].Layer(tile.id, tile.passable);
                    }
                }
            }

            ResultMap.Width = ResultMap.Tiles[0].Count;
            ResultMap.Height = ResultMap.Tiles.Count;
            return ResultMap;
        }
    }
}