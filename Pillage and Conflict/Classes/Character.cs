using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public int CharSpeed = 250;
        public int CharRow = 50;
        public int CharColumn = 50;
        public float Charx = 80 * 20;
        public float Chary = 80*20;
        public int ProjectileSpeed = 50;
        public int ProjectileSize = 5;
        public int Damage = 0;
        public Map CurrentMap;
        public Character()
        {
            CooldownTime = DateTime.Now;
            AttackSpeed = 100;
        }
        public void Update(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            var kstate = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                Chary -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Chary < 0)
                    Chary = 0;
            }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                Chary += (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Chary > CurrentMap.Height * 20 - 20)
                    Chary = CurrentMap.Height * 20 - 20;
            }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                Charx -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Charx < 0)
                    Charx = 0;
            }
            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                Charx += (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Charx > CurrentMap.Width * 20 - 20)
                    Charx = CurrentMap.Width * 20 - 20;
            }
            Charx += (float)(gamePadState.ThumbSticks.Left.X * CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Chary -= (float)(gamePadState.ThumbSticks.Left.Y * CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            CharRow = (int)Chary / 20;
            CharColumn = (int)Charx / 20;
            if (DateTime.Now > CooldownTime)
            {
                MouseState mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    double relx = mouse.X - GraphicsDevice.Viewport.Bounds.Width / 2;
                    double rely = mouse.Y - GraphicsDevice.Viewport.Bounds.Height / 2;

                    if (Math.Abs(relx) > Math.Abs(rely))
                    {
                        float scaledx;
                        if (relx > 0)
                            scaledx = 1;
                        else
                            scaledx = -1;
                        float scaledy = (float)-(rely / relx);
                        if (rely > 0 && relx > 0 || rely < 0 && relx > 0)
                            scaledy = -scaledy;
                        CurrentMap.Projectiles.Add(new Projectile(scaledx, scaledy, ProjectileSpeed, ProjectileSize, ProjectTexture, Damage, Charx, Chary, new Vector2(10, 6), 300));
                    }
                    else
                    {
                        float scaledx = (float)(relx / rely);
                        if (relx < 0 && rely < 0 || relx > 0 && rely < 0)
                            scaledx = -scaledx;
                        float scaledy;
                        if (rely > 0)
                            scaledy = 1;
                        else
                            scaledy = -1;
                        CurrentMap.Projectiles.Add(new Projectile(scaledx, scaledy, ProjectileSpeed, ProjectileSize, ProjectTexture, Damage, Charx, Chary, new Vector2(10, 6), 300));

                    }
                    CooldownTime = DateTime.Now.AddMilliseconds(AttackSpeed);
                }
                if (gamePadState.Triggers.Right > .5)
                {
                    CurrentMap.Projectiles.Add(new Projectile(gamePadState.ThumbSticks.Right.X, -gamePadState.ThumbSticks.Right.Y, ProjectileSpeed, ProjectileSize, ProjectTexture, Damage, Charx, Chary, new Vector2(10, 6), 300));
                    CooldownTime = DateTime.Now.AddMilliseconds(AttackSpeed);
                }
            }
        }

    }
}
