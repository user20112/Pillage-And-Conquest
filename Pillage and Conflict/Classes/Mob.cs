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
        public int Speed = 50;
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

        public void Update(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {

        }

        public int[] GetTile()
        {
            return new int[2] { 1 + (int)Chary / 20, (int)(Charx / 20) + 1 };
        }
    }
}