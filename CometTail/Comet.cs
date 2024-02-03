using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;
using System.Net.NetworkInformation;

namespace CometTail
{
    internal class Comet : GameObject
    {
        // Fields
        // Physics
        private Vector2 pos;
        private Vector2 velocity;
        private Vector2 acceleration;
        private float friction;

        // Device input
        private MouseState mouseState;
        private KeyboardState currentkbState;
        private KeyboardState previouskbState;
        private Keys currentKey;
        private Keys previousKey;

        // Properties
        /// <summary>
        /// Read and Write property for x position
        /// </summary>
        public float PositionX { get { return pos.X; } set { pos.X = value; } }

        /// <summary>
        /// Read and Write property for y positon
        /// </summary>
        public float PositionY { get { return pos.Y; } set { pos.Y = value; } }

        public float Velocity { set { velocity.X = value; } }

        // Constructor
        /// <summary>
        /// Instantiates Comet class fields
        /// </summary>
        /// <param name="texture">Comet Texture</param>
        /// <param name="position">Comet starting position</param>
        public Comet(Texture2D texture, Rectangle position) : base(texture, position)
        {
            this.texture = texture;
            pos.X = position.X;
            pos.Y = position.Y;

            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            friction = 0.01f;

            previousKey = Keys.D;
        }

        // Methods
        public override void Update(GameTime gameTime, float dt)
        {
            // Update the position every frame
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

            // Move the y pos of the comet with the y pos of the mouse
            pos.Y = mouseState.Y;

            // Update velocity
            velocity.X += acceleration.X * dt;

            // Update position based on velocity
            pos.X += velocity.X * dt;
            pos.Y += velocity.Y * dt;

            // Check Keyboard input //
            // D mash - Check for one frame of D being pressed
            if (currentkbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
            {
                // Will only affect movement if A was last pressed (Must mash A-D-A-D)
                if (previousKey != Keys.A)
                {
                    return;
                }

                // Accelerate on x-axis and set the current key
                acceleration.X += 5000f;
                currentKey = Keys.D;
            }
            // A mash -Check for one frame of A being pressed
            else if (currentkbState.IsKeyDown(Keys.A) && previouskbState.IsKeyUp(Keys.A))
            {
                // Will only affect movement if D was last pressed (Must mash A-D-A-D)
                if (previousKey != Keys.D)
                {
                    return;
                }

                // Accelerate on x-axis and set the current key
                acceleration.X += 5000f;
                currentKey = Keys.A;
            }
            else if (currentKey != Keys.None)
            {
                // Save the past current key for mash order check
                previousKey = currentKey;

                // Move the comet backwards when the player is not mashing
                acceleration.X = 0;
                velocity.X -= 4f;
            }

            // Deaccelerate beacuse of friction
            velocity.X -= (float)(velocity.X * friction);


            #region DEBUG
            // Pause here at any given moment
            if (currentkbState.IsKeyDown(Keys.U))
            {
                Console.WriteLine("Hello");
            }
            #endregion

            // Save past keyboard state for single press buttons
            previouskbState = currentkbState;
        }

        /// <summary>
        /// Draw method override to draw the comet
        /// </summary>
        /// <param name="sb">Sprite Batch</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, Color.White);
        }

    }
}
