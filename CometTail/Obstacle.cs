using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometTail
{
    internal class Obstacle
        : GameObject
    {
        // Fields
        private Vector2 pos;

        
        // Property

        // Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public Obstacle(Texture2D texture, Rectangle position) : base(texture, position)
        {
            pos.X = position.X;
            pos.Y = position.Y;

            radius = texture.Width / 2;
            
        }

        // Methods
        public override void Update(GameTime gameTime, float dt)
        {
            center.X = pos.X + (texture.Width / 2);
            center.Y = pos.Y + (texture.Height / 2);

            position.X = (int)pos.X;
            position.Y = (int)pos.Y;

            pos.X -= 0.5f;

            if (pos.X + texture.Width < 0)
            {
                pos.X = 500;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, Color.White);
        }
    }
}
