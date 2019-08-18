using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pillage_and_Conflict.Classes;
using System;

namespace Pillage_and_Conflict
{
    public class Mob
    {
        public Texture2D Texture;
        public Texture2D ProjectTexture;
        public double AttackSpeed;
        public DateTime CooldownTime;
        public int Speed = 250;
        public int Row = 50;
        public int Column = 50;
        public float Charx = 80 * 20;
        public float Chary = 80 * 20;
        public int ProjectileSpeed = 50;
        public int ProjectileSize = 5;
        public int Damage = 0;
        public Map CurrentMap;
        public Mob()
        {
            CooldownTime = DateTime.Now;
            AttackSpeed = 100;
        }

        //public void Update(GameTime gameTime, GraphicsDevice GraphicsDevice)
        //{
        //    int[] Position = GetTile();
        //    bool CanWalkUp = true;
        //    bool CanWalkRight = true;
        //    bool CanWalkLeft = true;
        //    bool CanWalkDown = true;
        //    foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
        //    {
        //        if (!tile.passable)
        //        {
        //            if (Chary % 20 < 10)
        //                CanWalkDown = false;
        //        }
        //    }
        //    foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
        //    {
        //        if (!tile.passable)
        //        {
        //            if (Chary % 20 > 10)
        //                CanWalkUp = false;
        //        }
        //    }
        //    foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
        //    {
        //        if (!tile.passable)
        //        {
        //            if (Charx % 20 > 10)
        //                CanWalkLeft = false;
        //        }
        //    }
        //    foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
        //    {
        //        if (!tile.passable)
        //        {
        //            if (Charx % 20 < 10)
        //                CanWalkRight = false;
        //        }
        //    }
        //    if (DateTime.Now > CooldownTime)
        //    {
        //        if (mouse.LeftButton == ButtonState.Pressed)
        //        {
        //            double relx = mouse.X - GraphicsDevice.Viewport.Bounds.Width / 2;
        //            double rely = mouse.Y - GraphicsDevice.Viewport.Bounds.Height / 2;

        //            if (Math.Abs(relx) > Math.Abs(rely))
        //            {
        //                float scaledx;
        //                if (relx > 0)
        //                    scaledx = 1;
        //                else
        //                    scaledx = -1;
        //                float scaledy = (float)-(rely / relx);
        //                if (rely > 0 && relx > 0 || rely < 0 && relx > 0)
        //                    scaledy = -scaledy;
        //                CurrentMap.Projectiles.Add(new Projectile(scaledx, scaledy, ProjectileSpeed, ProjectileSize, ProjectTexture, Damage, Charx, Chary, new Vector2(10, 6), 300));
        //            }
        //            else
        //            {
        //                float scaledx = (float)(relx / rely);
        //                if (relx < 0 && rely < 0 || relx > 0 && rely < 0)
        //                    scaledx = -scaledx;
        //                float scaledy;
        //                if (rely > 0)
        //                    scaledy = 1;
        //                else
        //                    scaledy = -1;
        //                CurrentMap.Projectiles.Add(new Projectile(scaledx, scaledy, ProjectileSpeed, ProjectileSize, ProjectTexture, Damage, Charx, Chary, new Vector2(10, 6), 300));
        //            }
        //            CooldownTime = DateTime.Now.AddMilliseconds(AttackSpeed);
        //        }
        //    }
        //}

        public int[] GetTile()
        {
            return new int[2] { 1 + (int)Chary / 20, (int)(Charx / 20) + 1 };
        }
    }
}