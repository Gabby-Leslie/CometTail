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

        // Comet variables
        private Texture2D cometTex;
        private Vector2 cometPos;
        private Rectangle cometRect;

        // State handling
        private GameState currentState;
        private GameState previousState;

        // Device states
        private KeyboardState currentkbState;
        private KeyboardState previouskbState;
        private Keys currentKey;
        private Keys prevKey;
        private MouseState mouseState;

        // Screen Resolution
        private int screenHeight;
        private int screenWidth;        

        // Physics
        private Vector2 acceleration;
        private Vector2 velocity;
        
        // ** DEBUG VARIABLES **
        // Debug Text
        private string posText;
        private string buttonText;
        private SpriteFont centaur32;

        private bool followMouse;

        // ** ASSETS ** 
        // Background
        // Comet
        private Comet comet;

        // Planets

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

            velocity = new Vector2(0,0);
            acceleration = new Vector2(0, 0);

            prevKey = Keys.D;

            followMouse = true;

            

            /*_graphics.ToggleFullScreen();*/

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cometTex = Content.Load<Texture2D>("Comet");

            cometRect = new Rectangle(40,
                                      _graphics.PreferredBackBufferHeight/2 - cometTex.Height,
                                      cometTex.Width,
                                      cometTex.Height);


            centaur32 = Content.Load<SpriteFont>("centaur-32");
            comet = new Comet(cometTex, cometRect);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentkbState = Keyboard.GetState();
            
            if (currentkbState.IsKeyDown(Keys.D))
            {
                currentKey = Keys.D;
            }
            else if (currentkbState.IsKeyDown(Keys.A))
            {
                currentKey = Keys.A;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            comet.Update(gameTime, dt);
            CheckBounds(comet);

            // DEBUG : Show position in text
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

            comet.Draw(_spriteBatch);

            _spriteBatch.DrawString(centaur32, posText, Vector2.Zero, Color.Black);
            _spriteBatch.DrawString(centaur32, buttonText, new Vector2(20, screenHeight - 40), Color.Black);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void CheckBounds(Comet comet)
        {
            // Y axis boundaries 
            if (comet.PositionY < 0 ||
                comet.PositionY + cometRect.Height > screenHeight)
            {
                if (comet.PositionY < 0)
                {
                    comet.PositionY = 0;
                    return;
                }

                comet.PositionY = screenHeight - cometTex.Height;
            }

            // X axis boundaries
            if (comet.PositionX < 0 || comet.PositionX + cometRect.Width > screenWidth)
            {
                if (comet.PositionX < 0)
                {
                    comet.PositionX = 0;
                    return;
                }

                comet.PositionX = 20;
            }
        }
    }
}