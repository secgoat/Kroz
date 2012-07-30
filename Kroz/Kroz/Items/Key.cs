using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Key : BaseItem
    {
         public Key(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = "You pick up a rusted old skeleton key";
         }

        public override void OnCollision(Player player)
        {
            player.Keys = 1;
            
        }

       
    }
}
