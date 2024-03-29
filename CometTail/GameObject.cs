﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CometTail
{
    internal abstract class GameObject
    {
        // Fields
        protected Texture2D texture;
        protected Rectangle position;
        /*protected float gravForce;*/
        protected float mass;

        protected Vector2 center;
        protected float radius;

        // Properties
        /// <summary>
        /// Read-only property for texture
        /// </summary>
        public Texture2D Texture 
        { 
            get { return texture; } 
        }

        /// <summary>
        /// Read-only property for position
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Center
        {
            get { return center; }
        }
        public float Radius
        {
            get { return radius; }
        }

        // Constructors
        /// <summary>
        /// Initializes fields of the game object class
        /// </summary>
        /// <param name="texture">Texture of game object</param>
        /// <param name="position">Position of game object</param>
        public GameObject(Texture2D texture, Rectangle position) 
        {
            this.texture = texture;
            this.position = position;
        }

        // Methods
        /// <summary>
        /// Update frames
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public abstract void Update(GameTime gameTime, float dt);

        public virtual void Instersects(GameObject collidable)
        {
            
        }

        /// <summary>
        /// Draw GameObject to the window
        /// </summary>
        /// <param name="sb">Sprite Batch</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                position,
                Color.White);
        }
    }
}
