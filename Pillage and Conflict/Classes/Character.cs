using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Character
    {
        public Texture2D Texture;
        public Texture2D ProjectTexture;
        public double AttackSpeed;
        public DateTime CooldownTime;
        public int CharSpeed = 75;
        public int CharRow = 50;
        public int CharColumn = 50;
        public float Charx = 80 * 20;
        public float Chary = 80 * 20;
        public int ProjectileSpeed = 50;
        public int ProjectileSize = 5;
        public int Damage = 0;
        public Character()
        {
            CooldownTime = DateTime.Now;
            AttackSpeed = 100;
        }
        
    }
}
