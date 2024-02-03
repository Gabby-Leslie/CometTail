using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;

namespace CometTail
{
    internal class Comet : GameObject
    {
        // Fields
        private Texture2D cometTexture;

        private Vector2 pos;
        private Vector2 velocity;
        private Vector2 acceleration;
        private float friction;

        private MouseState mouseState;
        private KeyboardState currentkbState;
        private KeyboardState previouskbState;
        private Keys currentKey;
        private Keys previousKey;

        public float PositionX { get { return pos.X; } set { pos.X = value; } }
        public float PositionY { get { return pos.Y; } set { pos.Y = value; } }

        // Constructor
        /// <summary>
        /// Instantiates Comet class fields
        /// </summary>
        /// <param name="texture">Comet Texture</param>
        /// <param name="position">Comet starting position</param>
        public Comet(Texture2D texture, Rectangle position) : base(texture, position)
        {
            this.cometTexture = texture;
            pos.X = position.X;
            pos.Y = position.Y;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;

            previousKey = Keys.D;
        }

        // Methods
        public override void Update(GameTime gameTime, float dt)
        {
            position.X = (int)pos.X;
            position.Y = (int)pos.Y;

            // Get update user device input
            currentkbState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            // Cap velocity at 1000
            if (velocity.X > 1000)
            {
                velocity.X = 1000;
            }

            // Save delta time per frame

            #region Debug - Mouse follow switch
            /*if (currentkbState.IsKeyDown(Keys.M) && previouskbState.IsKeyUp(Keys.M))
            {
                followMouse = !followMouse;
            }

            if (followMouse && mouseState.Y >= 0 && mouseState.Y <= screenHeight)
            {
                cometPos.Y = mouseState.Y;
            }*/
            #endregion

            pos.Y = mouseState.Y;

            // Update velocity
            velocity.X += acceleration.X * dt;

            // Updat position based on velocity
            pos.X += velocity.X * dt;
            pos.Y += velocity.Y * dt;

            // Check Keyboard input
            if (currentkbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
            {
                if (previousKey != Keys.A)
                {
                    return;
                }

                acceleration.X += 5000f;
                currentKey = Keys.D;
            }
            else if (currentkbState.IsKeyDown(Keys.A) && previouskbState.IsKeyUp(Keys.A))
            {
                if (previousKey != Keys.D)
                {
                    return;
                }

                acceleration.X += 5000f;
                currentKey = Keys.A;
            }
            else if (currentKey != Keys.None)
            {
                previousKey = currentKey;
                acceleration.X = 0;
                velocity.X -= 4f;
            }

            // Deaccelerate beacuse of friction
            velocity.X -= (float)(velocity.X * 0.01);


            #region DEBUG
            // Show position in text
            /*posText = "(" + ((int)cometPos.X) + ", " + ((int)cometPos.Y) + ") - " + prevKey;*/

            // Pause here at any given moment
            if (currentkbState.IsKeyDown(Keys.U))
            {
                Console.WriteLine("Hello");
            }
            #endregion

            previouskbState = currentkbState;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, Color.White);
        }

    }
}
