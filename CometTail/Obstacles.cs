using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometTail
{
    internal class Obstacles : GameObject
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
        public Obstacles(Texture2D texture, Rectangle position) : base(texture, position)
        {

        }

        // Methods
        public override void Update(GameTime gameTime, float dt)
        {
            throw new NotImplementedException();
        }
    }
}
