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
            List<Tile> Row = new List<Tile>();
            Map Map = new Map();
            string items = File.ReadAllText(Directory.GetCurrentDirectory() + "/" + MapName);
            string[] MapTiles = items.Split(',');
            for (int CurrentTile = 0; CurrentTile < MapTiles.Length - 1; CurrentTile++)
                if (CurrentTile % 321 == 0 && CurrentTile != 0)
                {
                    Map.Add(Row);
                    Row = new List<Tile>();
                    Row.Add(new Tile(Convert.ToInt32(MapTiles[CurrentTile]), Content));
                }
                else
                {
                    Row.Add(new Tile(Convert.ToInt32(MapTiles[CurrentTile]), Content));
                }
            return Map;
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
            for (int x = 0; x < 10; x++)
            {
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Dirt00" + x.ToString()));
            }
            CharModels.Add(Content.Load<Texture2D>("CharModels/orc00"));
            ProjectileTextures.Add(Content.Load<Texture2D>("ice_shard/7"));
            Character.ProjectTexture = ProjectileTextures[0];
            Character.Texture = CharModels[0];
            // TODO: use this.Content to load your game content here
            CurrentMap = LoadMap("Map.TXT");
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
            if (kstate.IsKeyDown(Keys.Up))
                Character.Chary -= (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Down))
                Character.Chary += (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Left))
                Character.Charx -= (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Right))
                Character.Charx += (int)(Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            Character.Charx += (float)(gamePadState.ThumbSticks.Left.X * Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Character.Chary -= (float)(gamePadState.ThumbSticks.Left.Y * Character.CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Character.CharRow = (int)Character.Chary / 20;
            Character.CharColumn = (int)Character.Charx / 20;
            MouseState mouse = Mouse.GetState();
            if (DateTime.Now > Character.CooldownTime)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    float Angle = (float)((180 / Math.PI) * Math.Atan2(mouse.Y - GraphicsDevice.Viewport.Bounds.Height / 2, mouse.X - GraphicsDevice.Viewport.Bounds.Width / 2));
                    Projectiles.Add(new Projectile(Angle, Character.ProjectileSpeed, Character.ProjectileSize, Character.ProjectTexture, Character.Damage, Character.Charx, Character.Chary));
                    Character.CooldownTime = DateTime.Now.AddMilliseconds(Character.AttackSpeed);
                }
                if (gamePadState.Triggers.Right > .5)
                {
                    float Angle = (float)((180 / Math.PI) * Math.Atan2(gamePadState.ThumbSticks.Right.Y, gamePadState.ThumbSticks.Right.X));
                    Projectiles.Add(new Projectile(Angle, Character.ProjectileSpeed, Character.ProjectileSize, Character.ProjectTexture, Character.Damage, Character.Charx, Character.Chary));
                    Character.CooldownTime = DateTime.Now.AddMilliseconds(Character.AttackSpeed);
                }
                foreach (Projectile projectile in Projectiles)
                    projectile.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Scale);
            DrawMap();
            spriteBatch.Draw(Character.Texture, new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2, GraphicsDevice.Viewport.Bounds.Height / 2), Color.White);
            foreach (Projectile projectile in Projectiles)
                DrawIfNearCharacter(projectile, Character);
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
                    spriteBatch.Draw(CurrentMap.Tiles[xMap][yMap].Texture, new Vector2(Relx, Rely), Color.White);
                    Rely += 20;
                }
                Rely = -20 - (int)Character.Chary % 20;
                Relx += 20;
            }
        }
        public void DrawIfNearCharacter(Projectile projectile, Character character)
        {
            int relx = (int)(projectile.Positionx - character.Charx);
            int rely = (int)(projectile.Positiony - character.Chary);
            if (relx < GraphicsDevice.Viewport.Bounds.Width / 2 && relx > -GraphicsDevice.Viewport.Bounds.Width / 2 && rely < GraphicsDevice.Viewport.Bounds.Height / 2 && rely > -GraphicsDevice.Viewport.Bounds.Height / 2)//if its on screen
                spriteBatch.Draw(projectile.Sprite, new Vector2(relx + GraphicsDevice.Viewport.Bounds.Width / 2, rely + GraphicsDevice.Viewport.Bounds.Height / 2), Color.White);
        }
    }
}
