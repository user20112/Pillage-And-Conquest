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
        public double AttackSpeed;
        public DateTime CooldownTime;
        public Character()
        {
            CooldownTime = DateTime.Now;
            AttackSpeed = 500;
        }
    }
}
