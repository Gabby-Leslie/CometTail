﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;

namespace CometTail
{
    /// <summary>
    /// States for each part of the Game loop
    /// Start -> Play -> GameOver
    /// Play -> Pause -> Settings -> Pause
    /// </summary>
    public enum GameState
    {
        mStart,
        mPause,
        mSettings,
        mGameOver,
        Play
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // State handling
        private GameState currentState;
        private GameState previousState;

        // Device states
        private KeyboardState currentkbState;
        private Keys currentKey;

        // Screen Resolution
        private int screenHeight;
        private int screenWidth;
        
        // ** ASSETS ** 
        // Background
        // Comet
        private Comet comet;

        private Texture2D cometTex;
        private Rectangle cometRect;

        // Planets
        private Obstacle planet;

        private Texture2D planetTex;
        private Rectangle planetRect;

        private List<Obstacle> planets;


        // ** DEBUG VARIABLES **
        // Debug Text
        private string posText;
        private string buttonText;
        public SpriteFont centaur32;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Change window size
            _graphics.PreferredBackBufferWidth = 1280;    // Width size
            _graphics.PreferredBackBufferHeight = 720;   // Height size
        }

        protected override void Initialize()
        {
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;

            /*_graphics.ToggleFullScreen();*/
            planets = new List<Obstacle>();
            Mouse.SetPosition(screenWidth / 2, screenHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Comet values
            cometTex = Content.Load<Texture2D>("Comet");
            cometRect = new Rectangle(40,                                   // X pos
                                      screenHeight/2 - cometTex.Height,     // Y pos
                                      cometTex.Width,                       // Rectangle Width
                                      cometTex.Height);                     // Rectangle Height


            // Initialize the font and the comet object            
            comet = new Comet(cometTex, cometRect);

            // Planet Values
            planetTex = Content.Load<Texture2D>("Obstacle");
            planetRect = new Rectangle(300,                                 // X pos
                                       screenHeight/2 + 100,   // Y pos
                                       planetTex.Width, planetTex.Height);  // Rectangle Width and Height

            planet = new Obstacle(planetTex, planetRect);

            centaur32 = Content.Load<SpriteFont>("centaur-32");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update keyboard state each frame for debug test
            currentkbState = Keyboard.GetState();

            // Save the current key being pressed for debug test
            if (currentkbState.IsKeyDown(Keys.D))
            {
                currentKey = Keys.D;
            }
            else if (currentkbState.IsKeyDown(Keys.A))
            {
                currentKey = Keys.A;
            }

            // Delta time variable
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update and check bounds of the comet
            comet.Update(gameTime, dt);
            planet.Update(gameTime, dt);

            // Bound checking and collision detection
            CheckBounds(comet);
            CheckInterect(comet, planet);

            // Planet gravity
            if ((comet.Center - planet.Center).Length() < planet.Radius * 2)
            {
                comet.ApplyForce((planet.Center - comet.Center)); 
            }

            // DEBUG PAUSE WITH B
            if (currentkbState.IsKeyDown(Keys.B))
            {
                System.Diagnostics.Debug.WriteLine("h");
            }

            // DEBUG : Show position and current button being mashed in text
            posText = "(" + ((int)comet.PositionX) + ", " + ((int)comet.PositionY) + ") - " + (int)comet.VelocityX + ", " + (int)comet.VelocityY;
            buttonText = currentKey.ToString();

            // Game Loop switch statement
            switch (currentState)
            {
                case GameState.mStart:
                    // TODO: Check for button presses
                    // TODO: Set starting variables if applicable
                    break;
                case GameState.Play:
                    // TODO: Update score
                    // TODO: Call comet update method
                    // TODO: Check for collisions (Determines death)
                    // TODO: Check for ESC for pause
                    break;
                case GameState.mPause:
                    // TODO: Check for button presses
                    break;
                case GameState.mSettings:
                    // TODO: Check for setting changes and update game settings
                    // TODO: Check for button presses
                    break;
                case GameState.mGameOver:
                    // TODO: Display score 
                    // TODO: Check for button presses
                    break;

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightCoral);

            _spriteBatch.Begin();

            // Draw the player comet
            comet.Draw(_spriteBatch);

            // Draw the planet
            planet.Draw(_spriteBatch);

            // Debug text
            _spriteBatch.DrawString(centaur32, posText, Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(centaur32, buttonText, new Vector2(20, screenHeight - 40), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Keep the Comet from going beyond the screen boundaries
        /// </summary>
        /// <param name="comet">Comet Gameobject</param>
        private void CheckBounds(Comet comet)
        {
            // Y axis boundaries 
            if (comet.PositionY < 0 ||
                comet.PositionY + cometRect.Height > screenHeight)
            {
                // Top boundary
                if (comet.PositionY < 0)
                {
                    comet.PositionY = 0;
                    comet.VelocityY = 0;
                    return;
                }

                // Bottom boundary 
                comet.PositionY = screenHeight - cometTex.Height;
            }

            // X axis boundaries
            if (comet.PositionX < 0 || comet.PositionX + cometRect.Width > screenWidth)
            {
                // Left Boundary
                if (comet.PositionX < 0)
                {
                    comet.PositionX = 0;
                    comet.VelocityX = 0;
                    return;
                }

                // TODO: Remove screen wrapping
                comet.PositionX = 20;
            }
        }

        /// <summary>
        /// Check if the comet and a planet collide
        /// </summary>
        /// <param name="obj1">Comet or planet</param>
        /// <param name="obj2">Comet or Planet</param>
        private void CheckInterect(GameObject obj1, GameObject obj2)
        {
            // Check if the distance between the centers are less than the sum of the radii
            if ((obj1.Center - obj2.Center).Length() < (obj1.Radius + obj2.Radius))
            {
                // If so, they are colliding
                //System.Diagnostics.Debug.WriteLine("Colliding");
            }
        }
    }
}