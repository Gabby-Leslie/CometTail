using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Policy;

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
        private Vector2 totalForce;

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

        public Vector2 Pos { get { return pos; } }

        public float Velocity { get { return velocity.X; } set { velocity.X = value; } }

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
            mass = 0.5f;
            friction = 0.01f;

            previousKey = Keys.D;

            radius = texture.Width / 2;
        }

        // Methods
        public override void Update(GameTime gameTime, float dt)
        {
            // Update the position every frame
            position.X = (int)pos.X;
            position.Y = (int)pos.Y;

            // Update the center coord every frame
            center.X = pos.X + (texture.Width / 2);
            center.Y = pos.Y + (texture.Height / 2);

            // Get update user device input
            currentkbState = Keyboard.GetState();
            mouseState = Mouse.GetState();

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
            /*pos.Y = mouseState.Y;*/

            // Update velocity
            /*velocity.X += acceleration.X * dt;*/
            velocity += acceleration * dt;

            // Cap velocity at 500
            if (velocity.X > 500)
            {
                velocity.X = 500;
            }

            // Update position based on velocity
            pos.X += velocity.X * dt;
            pos.Y += velocity.Y * dt;

            //ApplyForce(mouseState.Position.Y - pos.Y);

            #region // Check Keyboard input // 
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
                //velocity.X -= 4f;
            }
            #endregion

            if (currentkbState.IsKeyDown(Keys.Up))
            {
                pos.Y -= 5f;
            }
            else if (currentkbState.IsKeyDown(Keys.Down))
            {
                pos.Y += 5f;
            }

            // Deaccelerate beacuse of friction
            velocity.X -= (float)(velocity.X * friction);



            // *** Build the totalForce
            // TODO add forces the comet experience to get total force


            ApplyForce(totalForce);



            #region DEBUG
            // Pause here at any given moment
            if (currentkbState.IsKeyDown(Keys.U))
            {
                System.Diagnostics.Debug.WriteLine("Hello");
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

        public void CalculateForce(Vector2 force)
        {
            totalForce += force;
        }

        public void ApplyForce(Vector2 force)
        {
            acceleration = force / mass;
        }

        public void ApplyForce(float force)
        {
            acceleration.Y = force / mass;
        }

    }
}
