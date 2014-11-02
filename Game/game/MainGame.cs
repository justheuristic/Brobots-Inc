#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using game.IO;
using game.Objects;
using game.Logic;
using game.Packages;
#endregion

namespace game
{

    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public int ScreenWidth;
        public int ScreenHeight;

        public ScreenManager screenManager;
        public InputManager inputManager;
        public AgentManager agentManager;
        public ObjectManager objectManager;

        public MainGame()
        {
            screenManager = new ScreenManager(this);
            inputManager = new InputManager(this);
            agentManager = new AgentManager(this);
            objectManager = new ObjectManager(this);
            _graphics = new GraphicsDeviceManager(this);
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

            ScreenWidth = GraphicsDevice.Viewport.Width;
            ScreenHeight = GraphicsDevice.Viewport.Height;

            #region sandbox area

            LevelTest.load(this);

            #endregion end of sandbox area

            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            screenManager.LoadTextures(Content, _spriteBatch, GraphicsDevice);
            SoundManager.LoadSounds(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        float lastUpdateTime = 0;
        float lastDrawTime = 0;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float dtime = currentTime- lastUpdateTime;
            inputManager.Loop(dtime);
            agentManager.Loop(dtime);
            objectManager.Loop(dtime);

            // TODO: Add your update logic here
            ScreenWidth = GraphicsDevice.Viewport.Width;
            ScreenHeight = GraphicsDevice.Viewport.Height;
            base.Update(gameTime);
            
            lastUpdateTime = currentTime;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float dtime = currentTime-lastDrawTime;

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            screenManager.Loop(dtime);
            _spriteBatch.End();
            base.Draw(gameTime);
            lastDrawTime = currentTime;
        }
    }
}
