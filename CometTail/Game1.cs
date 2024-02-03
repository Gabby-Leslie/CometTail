using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;

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


        // ** DEBUG VARIABLES **
        // Debug Text
        private string posText;
        private string buttonText;
        private SpriteFont centaur32;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Change window size
            _graphics.PreferredBackBufferWidth = 1280;    // Width size
            _graphics.PreferredBackBufferHeight = 720;   // Height size
        }

        protected override void Initialize()
        {
            screenHeight = _graphics.PreferredBackBufferHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;

            /*_graphics.ToggleFullScreen();*/

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Comet values
            cometTex = Content.Load<Texture2D>("Comet");
            cometRect = new Rectangle(40,
                                      _graphics.PreferredBackBufferHeight/2 - cometTex.Height,
                                      cometTex.Width,
                                      cometTex.Height);


            // Initialize the font and the comet object
            centaur32 = Content.Load<SpriteFont>("centaur-32");
            comet = new Comet(cometTex, cometRect);
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
            CheckBounds(comet);

            // DEBUG : Show position and current button being mashed in text
            posText = "(" + ((int)comet.PositionX) + ", " + ((int)comet.PositionY) + ")";
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
                    comet.Velocity = 0;
                    return;
                }

                // TODO: Remove screen wrapping
                comet.PositionX = 20;
            }
        }
    }
}