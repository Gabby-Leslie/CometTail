using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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

        // Velocity Caps
        private Vector2 vCapMin;
        private Vector2 vCapMax;

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

        /// <summary>
        /// Read and Write property for x velocity
        /// </summary>
        public float VelocityX { get { return velocity.X; } set { velocity.X = value; } }

        /// <summary>
        /// Read and Write property for y velocity
        /// </summary>
        public float VelocityY { get { return velocity.Y; } set { velocity.Y = value; } }

        // Constructor
        /// <summary>
        /// Instantiates Comet class fields
        /// </summary>
        /// <param name="texture">Comet Texture</param>
        /// <param name="position">Comet starting position</param>
        public Comet(Texture2D texture, Rectangle position) : base(texture, position)
        {
            // Visuals
            this.texture = texture;
            pos.X = position.X;
            pos.Y = position.Y;

            // Physics
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            mass = 1f;
            radius = texture.Width / 2;
            friction = 0.02f;

            // Velocity caps
            vCapMax = new Vector2(400, 400);
            vCapMin = new Vector2(-200, -400);

            previousKey = Keys.D;
        }

        // Methods
        #region Core Methods

        public override void Update(GameTime gameTime, float dt)
        {
            // Get update user device input
            currentkbState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            // Update the rectangle position every frame
            position.X = (int)pos.X;
            position.Y = (int)pos.Y;

            // Update the center coord every frame
            center.X = pos.X + (texture.Width / 2);
            center.Y = pos.Y + (texture.Height / 2);

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
            #region DEBUG - Pause W/ U
            // Pause here at any given moment
            if (currentkbState.IsKeyDown(Keys.U))
            {
                System.Diagnostics.Debug.WriteLine("Hello");
            }
            #endregion

            // Update velocity
            velocity += acceleration * dt;

            // Velocity Caps
            CapVelocity(velocity);
            // Move the comet
            pos.X += velocity.X * dt;
            pos.Y += velocity.Y * dt;

            // Follow mouse
            ApplyForceY((mouseState.Position.Y - pos.Y) * 5);

            #region // Check Keyboard input // 
            // D mash - Check for one frame of D being pressed
            if (currentkbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
            {
                // Will only affect movement if A was last pressed (Must mash A-D-A-D)
                if (previousKey != Keys.A)
                {
                    return;
                }

                // X Axis Velocity Caps
                CapVelocity(velocity.X);

                // Accelerate on x-axis and set the current key
                acceleration.X = 5000f;
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

                // X Axis Velocity Caps
                CapVelocity(velocity.X);

                // Accelerate on x-axis and set the current key
                acceleration.X = 5000f;
                currentKey = Keys.A;
            }
            else if (currentKey != Keys.None)
            {
                // Save the past current key for mash order check
                previousKey = currentKey;

                // Move the comet backwards when the player is not mashing
                acceleration.X = 0;
                velocity.X -= 1f;

            }
            #endregion

            // Deaccelerate due to friction
            velocity -= velocity * friction;

            // *** Build the totalForce
            // TODO add forces the comet experience to get total force
            //ApplyForceY(totalForce);

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

        #endregion

        #region Other Methods

        /// <summary>
        /// Apply force to complete Vector2 movement
        /// </summary>
        /// <param name="force">Vector force</param>
        public void ApplyForce(Vector2 force)
        {
            acceleration += force / mass;
            CapVelocity(velocity);

        }

        /// <summary>
        /// Apply force to y movement
        /// </summary>
        /// <param name="force">Y axis force</param>
        public void ApplyForceY(float force)
        {
            acceleration.Y = force / mass;

            // Y Axis Velocity Caps
            CapVelocity(velocity.Y);
        }

        /// <summary>
        /// Apply force to x movement
        /// </summary>
        /// <param name="force">X axis force</param>
        public void ApplyForceX(float force)
        {
            acceleration.Y = force / mass;

            // X Axis Velocity Caps
            CapVelocity(velocity.X);
        }

        /// <summary>
        /// Caps velocity on one axis
        /// </summary>
        /// <param name="axis">Single Axis capping</param>
        public void CapVelocity(float Velocity)
        {
            if (Velocity == velocity.X) {
                // X Axis Velocity Caps
                if (velocity.X > vCapMax.X || velocity.X < vCapMin.X)
                {
                    if (velocity.X < vCapMin.X)
                    {
                        velocity.X = vCapMin.X;
                        return;
                    }

                    velocity.X = vCapMax.X;
                }
            }
            else
            {
                // Y Axis Velocity Caps
                if (velocity.Y > vCapMax.Y || velocity.Y < vCapMin.Y)
                {
                    if (velocity.Y < vCapMin.Y)
                    {
                        velocity.Y = vCapMin.Y;
                        return;
                    }

                    velocity.Y = vCapMax.Y;
                }
            }
        }
        
        /// <summary>
        /// Caps velocity entirely
        /// </summary>
        /// <param name="Velocity">Velocity vector</param>
        public void CapVelocity(Vector2 Velocity)
        {
            // X and Y axis capping
            CapVelocity(velocity.X);
            CapVelocity(velocity.Y);
        }

        #endregion
    }
}
