using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillage_and_Conflict
{
    public class Projectile
    {
        public Texture2D Sprite;
        public float Angle;
        public int Speed;
        public int Size;
        public int Damage;
        public float Positionx;
        public float Positiony;
        public double xComponent;
        public double yComponent;
        public Projectile(float angle, int speed, int size, Texture2D texture, int damage, float Startx, float Starty)
        {
            Angle = angle;
            Speed = speed;
            Size = size;
            Sprite = texture;
            Damage = damage;
            Positionx = Startx;
            Positiony = Starty;
            xComponent = Math.Cos(Angle);
            yComponent = Math.Sin(Angle);
        }
        public void Update(double time)
        {
            Positionx += (float)(Speed * xComponent * time);
            Positiony += (float)(Speed * yComponent * time);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
