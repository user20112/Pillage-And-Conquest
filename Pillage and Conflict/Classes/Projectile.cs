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
        public Vector2 StartPosition;
        public Vector2 Velocity;
        public Vector2 Position;
        public Vector2 Origin;
        public Character Owner;
        public float Range;
        public bool Exists;
        public Projectile(float x, float y, int speed, int size, Texture2D texture, int damage, float Startx, float Starty, Vector2 origin,float range)
        {
            Origin = origin;
            Speed = speed;
            Size = size;
            Sprite = texture;
            Damage = damage;
            Velocity = new Vector2(x, y);
            Angle = (float)(Math.Atan2(y, x));
            StartPosition = new Vector2(Startx, Starty);
            Position = new Vector2(Startx, Starty);
            Exists = true;
            Range = range;
        }
        public void Update(double time)
        {
            Position.X += (float)(Speed * Velocity.X * time);
            Position.Y += (float)(Speed * Velocity.Y * time);
            if (Range<Vector2.Distance(StartPosition, new Vector2(Position.X, Position.Y)))
            {
                Exists = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
