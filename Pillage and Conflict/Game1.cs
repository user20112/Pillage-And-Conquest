using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pillage_and_Conflict
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PillageandConflict : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map CurrentMap;
        int DisplayRow = 0;
        int DisplayColumn = 0;
        int XViewCount = 50;
        int YViewCount = 50;
        int ZoomLevel = 1;
        public static List<Texture2D> Textures;
        public static List<Texture2D> ProjectileTextures;
        public static List<Texture2D> CharModels;
        public Character Character;
        const int TargetWidth = 1280;
        const int TargetHeight = 640;
        Matrix Scale;
        List<Projectile> Projectiles;

        public Map LoadMap(string MapName)
        {
            List<Tiles> Row = new List<Tiles>();
            Map Map = new Map();
            string items = File.ReadAllText(Directory.GetCurrentDirectory() + "/" + MapName);
            string[] MapTiles = items.Split(',');
            for (int CurrentTile = 0; CurrentTile < MapTiles.Length - 1; CurrentTile++)
                if (CurrentTile % 321 == 0 && CurrentTile != 0)
                {
                    Map.Add(Row);
                    Row = new List<Tiles>();
                    Row.Add(new Tiles(Convert.ToInt32(MapTiles[CurrentTile])));
                }
                else
                {
                    Row.Add(new Tiles(280));
                }
            return Map;
        }
        public Map generatemap(int Height, int Width)
        {
            Map Map = new Map();
            for (int y = 0; y < Height; y++)
            {
                List<Tiles> Row = new List<Tiles>();
                for (int x = 0; x < Width; x++)
                {
                    Row.Add(new Tiles(280));
                }
                Map.Tiles.Add(Row);
            }
            Random rand = new Random();
            for (int x = 0; x < Height / 80; x++)
            {
                Map = GenerateDirtpatch(Map, rand.Next(50, Height - 50), rand.Next(50, Width - 50), rand.Next(40, 50));
            }
            for (int x = 0; x < Height / 5; x++)
            {
                Map = GenerateSandpatch(Map, rand.Next(50, Height - 50), rand.Next(50, Width - 50), rand.Next(40, 50));
            }
            for (int x = 0; x < Height / 20; x++)
            {
                Map = GeneratePond(Map, rand.Next(10, Height - 10), rand.Next(10, Width - 10), rand.Next(5, 10));
            }
            Map.Width = Width;
            Map.Height = Height;
            return Map;
        }
        public Map GeneratePond(Map map, int positionx, int positiony, int width)
        {
            for (int x = 0; x < width / 3; x++)
            {
                for (int y = 0; y < width / 3; x++)
                {
                    if (y == 0)
                    {
                        if (x < (width / 3) * 2 && x > (width / 3))//Second third
                        {

                        }
                    }
                    else
                    {
                        if (x == 0)
                        {
                            if (x < (width / 3) * 2 && x > (width / 3))//Second third
                            {

                            }
                        }
                        else
                        {

                            if (x <= (width / 3))//first third
                            {

                            }
                            if (x < (width / 3) * 2 && x > (width / 3))//Second third
                            {

                            }
                            if (x >= (width / 3) * 2)//Third third
                            {

                            }
                        }
                    }
                }
            }
            return map;
        }
        public Map GenerateDirtpatch(Map map, int position, int positiony, int width)
        {

            return map;
        }
        public Map GenerateSandpatch(Map map, int position, int positiony, int width)
        {

            return map;
        }
        public PillageandConflict()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            Projectiles = new List<Projectile>();
            float scaleX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / TargetWidth;
            float scaleY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / TargetHeight;
            Scale = Matrix.CreateScale(new Vector3(scaleX / ZoomLevel, scaleY / ZoomLevel, 1));
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Character = new Character();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Textures = new List<Texture2D>();
            ProjectileTextures = new List<Texture2D>();
            CharModels = new List<Texture2D>();
            for (int x = 0; x < 140; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 3; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Dirt" + temp));
            }
            for (int x = 0; x < 140; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 3; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Grass" + temp));
            }
            Textures.Add(Content.Load<Texture2D>("BaseTiles/Grass233"));
            for (int x = 0; x < 375; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 4; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Water" + temp));
            }
            CharModels.Add(Content.Load<Texture2D>("CharModels/orc00"));
            ProjectileTextures.Add(Content.Load<Texture2D>("Projectiles/IceShard"));
            Character.ProjectTexture = ProjectileTextures[0];
            Character.Texture = CharModels[0];
            // TODO: use this.Content to load your game content here
            //CurrentMap = LoadMap("Map.TXT");
            CurrentMap = generatemap(160, 160);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                Character.Chary -= (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Character.Chary < 0)
                    Character.Chary = 0;
            }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                Character.Chary += (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Character.Chary > CurrentMap.Height * 20)
                    Character.Chary = CurrentMap.Height * 20;
            }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                Character.Charx -= (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Character.Charx > CurrentMap.Width * 20)
                    Character.Charx = CurrentMap.Width * 20;
            }
            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                Character.Charx += (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                if (Character.Charx < 0)
                    Character.Charx = 0;
            }
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            Character.Charx += (float)(gamePadState.ThumbSticks.Left.X * Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Character.Chary -= (float)(gamePadState.ThumbSticks.Left.Y * Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Character.CharRow = (int)Character.Chary / 20;
            Character.CharColumn = (int)Character.Charx / 20;
            if (DateTime.Now > Character.CooldownTime)
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
                        Projectiles.Add(new Projectile(scaledx, scaledy, Character.ProjectileSpeed, Character.ProjectileSize, Character.ProjectTexture, Character.Damage, Character.Charx, Character.Chary, new Vector2(10, 6), 300));
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
                        Projectiles.Add(new Projectile(scaledx, scaledy, Character.ProjectileSpeed, Character.ProjectileSize, Character.ProjectTexture, Character.Damage, Character.Charx, Character.Chary, new Vector2(10, 6), 300));

                    }
                    Character.CooldownTime = DateTime.Now.AddMilliseconds(Character.AttackSpeed);
                }
                if (gamePadState.Triggers.Right > .5)
                {
                    Projectiles.Add(new Projectile(gamePadState.ThumbSticks.Right.X, -gamePadState.ThumbSticks.Right.Y, Character.ProjectileSpeed, Character.ProjectileSize, Character.ProjectTexture, Character.Damage, Character.Charx, Character.Chary, new Vector2(10, 6), 300));
                    Character.CooldownTime = DateTime.Now.AddMilliseconds(Character.AttackSpeed);
                }
            }
            foreach (Projectile projectile in Projectiles)
                projectile.Update(gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            List<Projectile> ToBeRemoved = new List<Projectile>();
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Scale);
            DrawMap();
            spriteBatch.Draw(Character.Texture, new Rectangle(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2, 20, 20), null, Color.White, 0, new Vector2(10, 10), SpriteEffects.None, 0);
            foreach (Projectile projectile in Projectiles)
                if (projectile.Exists)
                    DrawIfNearCharacter(projectile, Character);
                else
                    ToBeRemoved.Add(projectile);
            foreach (Projectile projectile in ToBeRemoved)
                Projectiles.Remove(projectile);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawMap()
        {
            int ScreenStartX = Character.CharColumn - XViewCount / 2;
            int ScreenStartY = Character.CharRow - XViewCount / 2;
            int Relx = -20 - (int)Character.Charx % 20;
            int Rely = -20 - (int)Character.Chary % 20;
            for (int xMap = ScreenStartX; xMap < ScreenStartX + XViewCount; xMap++)
            {
                for (int yMap = ScreenStartY; yMap < ScreenStartY + YViewCount; yMap++)
                {
                    if (xMap < CurrentMap.Tiles.Count)
                        if (CurrentMap.Tiles[xMap].Count > yMap)
                            foreach (Texture2D Texture in CurrentMap.Tiles[xMap][yMap].Textures)
                                spriteBatch.Draw(Texture, new Vector2(Relx, Rely), Color.White);
                    Rely += 20;
                }
                Rely = -20 - (int)Character.Chary % 20;
                Relx += 20;
            }
        }
        public void DrawIfNearCharacter(Projectile projectile, Character character)
        {
            int relx = (int)(projectile.Position.X - character.Charx);
            int rely = (int)(projectile.Position.Y - character.Chary);
            if (relx < GraphicsDevice.Viewport.Bounds.Width / 2 && relx > -GraphicsDevice.Viewport.Bounds.Width / 2 && rely < GraphicsDevice.Viewport.Bounds.Height / 2 && rely > -GraphicsDevice.Viewport.Bounds.Height / 2)//if its on screen
                spriteBatch.Draw(projectile.Sprite, new Rectangle(relx + (GraphicsDevice.Viewport.Bounds.Width / 2), rely + (GraphicsDevice.Viewport.Bounds.Height / 2), 20, 12), null, Color.White, projectile.Angle, new Vector2(0, 0), SpriteEffects.None, 0);

        }
    }
}
