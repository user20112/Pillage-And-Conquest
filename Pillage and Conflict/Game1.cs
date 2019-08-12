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
        int CharRow = 50;
        int CharColumn = 50;
        int DisplayRow = 0;
        int DisplayColumn = 0;
        int XViewCount = 50;
        int YViewCount = 50;
        int CharSpeed = 75;
        float Charx = 160 * 20;
        float Chary = 160 * 20;
        int ZoomLevel = 1;
        public static List<Texture2D> Textures;
        public Character Character;
        const int TargetWidth = 1280;
        const int TargetHeight = 640;
        Matrix Scale;

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
            for (int x = 0; x < 10; x++)
            {
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Dirt00" + x.ToString()));
            }
            Character.Texture = Content.Load<Texture2D>("CharModels/orc00");
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
                Chary -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Down))
                Chary += (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Left))
                Charx -= (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            if (kstate.IsKeyDown(Keys.Right))
                Charx += (int)(CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            var gamePadState = GamePad.GetState(PlayerIndex.One);
            Charx += (float)(gamePadState.ThumbSticks.Left.X * CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            Chary -= (float)(gamePadState.ThumbSticks.Left.Y * CharSpeed * gameTime.ElapsedGameTime.TotalSeconds);
            CharRow = (int)Chary / 20;
            CharColumn = (int)Charx / 20;
            MouseState mouse = Mouse.GetState();
            if (DateTime.Now>Character.CooldownTime)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    float Angle = (float)((180 / Math.PI) * Math.Atan2(mouse.Y - GraphicsDevice.Viewport.Bounds.Height / 2, mouse.X - GraphicsDevice.Viewport.Bounds.Width / 2));
                    Character.CooldownTime = DateTime.Now;
                    Character.CooldownTime.AddMilliseconds(Character.AttackSpeed);
                }
                if (gamePadState.Triggers.Right > .5)
                {
                    float Angle = (float)((180 / Math.PI) * Math.Atan2(gamePadState.ThumbSticks.Right.Y, gamePadState.ThumbSticks.Right.X));
                    Character.CooldownTime = DateTime.Now.AddMilliseconds(Character.AttackSpeed);
                }
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
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawMap()
        {
            int ScreenStartX = CharColumn - XViewCount / 2;
            int ScreenStartY = CharRow - XViewCount / 2;
            int Relx = -20 - (int)Charx % 20;
            int Rely = -20 - (int)Chary % 20;
            for (int xMap = ScreenStartX; xMap < ScreenStartX + XViewCount; xMap++)
            {
                for (int yMap = ScreenStartY; yMap < ScreenStartY + YViewCount; yMap++)
                {
                    spriteBatch.Draw(CurrentMap.Tiles[xMap][yMap].Texture, new Vector2(Relx, Rely), Color.White);
                    Rely += 20;
                }
                Rely = -20 - (int)Chary % 20;
                Relx += 20;
            }
        }
    }
}
