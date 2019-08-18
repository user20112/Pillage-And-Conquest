using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pillage_and_Conflict.Classes;
using System;

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
        public float Chary = 80 * 20;
        public int ProjectileSpeed = 50;
        public int ProjectileSize = 5;
        public int Damage = 0;
        public Map CurrentMap;
        private int XViewCount = 40;
        private int YViewCount = 24;

        public Character()
        {
            CooldownTime = DateTime.Now;
            AttackSpeed = 100;
        }

        public void Update(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            var kstate = Keyboard.GetState();
            int[] Position = GetTile();
            bool CanWalkUp = true;
            bool CanWalkRight = true;
            bool CanWalkLeft = true;
            bool CanWalkDown = true;
            foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
            {
                if (!tile.passable)
                {
                    if (Chary % 20 < 10)
                        CanWalkDown = false;
                }
            }
            foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
            {
                if (!tile.passable)
                {
                    if (Chary % 20 > 10)
                        CanWalkUp = false;
                }
            }
            foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
            {
                if (!tile.passable)
                {
                    if (Charx % 20 > 10)
                        CanWalkLeft = false;
                }
            }
            foreach (Tile tile in CurrentMap.Tiles[Position[0]][Position[1]].tiles)
            {
                if (!tile.passable)
                {
                    if (Charx % 20 < 10)
                        CanWalkRight = false;
                }
            }
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            if (CanWalkUp && ((kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))))
            {
                Chary -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Chary < 0)
                    Chary = 0;
            }
            if (CanWalkDown && (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)))
            {
                Chary += (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Chary > CurrentMap.Height * 20 - 20)
                    Chary = CurrentMap.Height * 20 - 20;
            }
            if (CanWalkLeft && (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)))
            {
                Charx -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Charx < 0)
                    Charx = 0;
            }
            if (CanWalkRight && (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)))
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

        public int[] GetTile()
        {
            return new int[2] { 1 + (int)Chary / 20, (int)(Charx / 20) + 1 };
        }

        public void DrawMap(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            spriteBatch.Draw(PillageandConflict.Textures[0], new Rectangle(-(int)Charx % 20 + GraphicsDevice.Viewport.Bounds.Width / 2, -(int)Chary % 20 + GraphicsDevice.Viewport.Bounds.Height / 2, 20, 20), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .5f);
            int ScreenStartX = CharColumn - XViewCount / 2;
            int ScreenStartY = CharRow - YViewCount / 2;
            int Relx = -20 - (int)Charx % 20;
            for (int xMap = ScreenStartX; xMap <= ScreenStartX + XViewCount + 1; xMap++)
            {
                int Rely = -20 - (int)Chary % 20;
                for (int yMap = ScreenStartY; yMap <= ScreenStartY + YViewCount + 1; yMap++)
                {
                    if (CurrentMap.Height > yMap && yMap > 0 && xMap > 0 && CurrentMap.Width > xMap)
                        foreach (Tile Tile in CurrentMap.Tiles[xMap][yMap].tiles)
                        {
                            spriteBatch.Draw(Tile.Texture, new Rectangle(Relx, Rely, Tile.Width, Tile.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, Tile.layer);
                        }
                    Rely += 20;
                }
                Relx += 20;
            }
        }
    }
}