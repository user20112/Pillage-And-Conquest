using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pillage_and_Conflict
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PillageandConflict : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Map CurrentMap;
        private int DisplayRow = 0;
        private int DisplayColumn = 0;
        private int ZoomLevel = 1;
        public static List<Texture2D> Textures;
        public static List<Texture2D> ProjectileTextures;
        public static List<Texture2D> CharModels;
        public Character Character;
        private const int TargetWidth = 1280;
        private const int TargetHeight = 640;
        private Matrix Scale;

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
            for (int x = 0; x < 46; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 2; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Ice" + temp));
            }
            for (int x = 0; x < 46; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 2; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/White" + temp));
            }
            for (int x = 0; x < 12; x++)
            {
                string temp = x.ToString();
                for (int y = temp.Length; y < 2; y++)
                    temp = "0" + temp;
                Textures.Add(Content.Load<Texture2D>("BaseTiles/Cliff" + temp));
            }
            Textures.Add(Content.Load<Texture2D>("Misc/Cave"));
            Textures.Add(Content.Load<Texture2D>("Misc/Portal"));
            Textures.Add(Content.Load<Texture2D>("Misc/Portal2"));
            CharModels.Add(Content.Load<Texture2D>("CharModels/orc00"));
            ProjectileTextures.Add(Content.Load<Texture2D>("Projectiles/IceShard"));
            Character.ProjectTexture = ProjectileTextures[0];
            Character.Texture = CharModels[0];
            // TODO: use this.Content to load your game content here
            CurrentMap = Map.generatemap(160, 160);
            CurrentMap.Tiles[80][80].Layer(750, false);
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
            Character.DrawMap(spriteBatch, GraphicsDevice);
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

        public void DrawIfNearCharacter(Projectile projectile, Character character)
        {
            int relx = (int)(projectile.Position.X - character.Charx);
            int rely = (int)(projectile.Position.Y - character.Chary);
            if (relx < GraphicsDevice.Viewport.Bounds.Width / 2 && relx > -GraphicsDevice.Viewport.Bounds.Width / 2 && rely < GraphicsDevice.Viewport.Bounds.Height / 2 && rely > -GraphicsDevice.Viewport.Bounds.Height / 2)//if its on screen
                projectile.Draw(spriteBatch, GraphicsDevice,relx,rely);
        }
        public void DrawMiniMap()
        {

        }
    }
}