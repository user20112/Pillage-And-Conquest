using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pillage_and_Conflict.Classes;
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
        int XViewCount = 40;
        int YViewCount = 24;
        int ZoomLevel = 1;
        public static List<Texture2D> Textures;
        public static List<Texture2D> ProjectileTextures;
        public static List<Texture2D> CharModels;
        public Character Character;
        const int TargetWidth = 1280;
        const int TargetHeight = 640;
        Matrix Scale;

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
                    Row.Add(new Tiles(Convert.ToInt32(MapTiles[CurrentTile]),true));
                }
                else
                {
                    Row.Add(new Tiles(280,true));
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
            this.IsMouseVisible = true;
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
            CurrentMap = Map.generatemap(160, 160);
            Character.CurrentMap = CurrentMap;
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
            Character.Update(gameTime, GraphicsDevice);
            foreach (Projectile projectile in CurrentMap.Projectiles)
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
            foreach (Projectile projectile in CurrentMap.Projectiles)
                if (projectile.Exists)
                    DrawIfNearCharacter(projectile, Character);
                else
                    ToBeRemoved.Add(projectile);
            foreach (Projectile projectile in ToBeRemoved)
                CurrentMap.Projectiles.Remove(projectile);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawMap()
        {
            spriteBatch.Draw(Textures[0], new Rectangle(-(int)Character.Charx % 20 + GraphicsDevice.Viewport.Bounds.Width / 2, -(int)Character.Chary % 20 + GraphicsDevice.Viewport.Bounds.Height / 2, 20, 20), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .5f);
            int ScreenStartX = Character.CharColumn - XViewCount / 2;
            int ScreenStartY = Character.CharRow - YViewCount / 2;
            int Relx = -20-(int)Character.Charx % 20;
            for (int xMap = ScreenStartX; xMap <= ScreenStartX + XViewCount + 1; xMap++)
            {
                int Rely = -20 - (int)Character.Chary % 20;
                for (int yMap = ScreenStartY; yMap <= ScreenStartY + YViewCount+1; yMap++)
                {
                    if (CurrentMap.Height > yMap && yMap > 0 && xMap > 0 && CurrentMap.Width > xMap)
                        foreach (Tile Tile in CurrentMap.Tiles[xMap][yMap].tiles)
                        {
                            if(CurrentMap.Tiles[xMap][yMap].tiles.Count>1)
                            {

                            }
                            spriteBatch.Draw(Tile.Texture, new Rectangle(Relx, Rely, Tile.Width, Tile.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, Tile.layer);
                        }
                    Rely += 20;
                }
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
