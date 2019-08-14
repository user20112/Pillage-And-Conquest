using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Map
    {
        public int Width;
        public int Height;
        public List<List<Tiles>> Tiles = new List<List<Tiles>>();
        public List<List<Tiles>> Structures = new List<List<Tiles>>();
        public List<Projectile> Projectiles;
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
                    Row.Add(new Tiles(280));
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
                for (int x = 0; x < Height / 2; x++)
                {
                    Map.GeneratePond(rand.Next(10, Height - 10), rand.Next(10, Width - 10), rand.Next(5, 10));
                }
            }
            Map.Width = Width;
            Map.Height = Height;
            return Map;
        }
        public void GeneratePond(int positionx, int positiony, int width)
        {
            bool FirstWater = true;
            bool LastWater = false;
            for (int x = positionx; x < (width / 3) + positionx; x++)
            {
                for (int y = positiony; y < width + positiony / 3; y++)
                {
                    if (y == positiony)
                    {
                        if (x < (width / 3) * 2 + positionx && x > (width / 3) + positionx)//Second third
                        {
                            if (FirstWater)
                                Tiles[x][y].Layer(293);
                            else
                                if (x == width / 3 + positionx)//last water
                                Tiles[x][y].Layer(295);
                            else
                                Tiles[x][y].Layer(294);
                        }
                    }
                    else
                    {
                        //if (x == 0)
                        //{
                        //    if (x < (width / 3) * 2 + positionx && x > (width / 3) + positionx)//Second third
                        //    {

                        //    }
                        //}
                        //else
                        //{

                        //    if (x <= (width / 3) + positionx)//first third
                        //    {

                        //    }
                        //    if (x < (width / 3) * 2 + positionx && x > (width / 3) + positionx)//Second third
                        //    {

                        //    }
                        //    if (x >= (width / 3) * 2 + positionx)//Third third
                        //    {

                        //    }
                        //}
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
    }
}
