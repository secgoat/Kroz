using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Door : Wall
    {

         public Door(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame, bool visible)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, false, visible, Color.White)
        {
            this.m_Visible = visible;
         }

        public bool Unlock(Player player)
        {
            if (!m_Visible)
            {
                m_Visible = true;
                return false;
            }
            else
            {
                int x = player.Keys;
                if (x > 0)
                {
                    player.Keys = -1;
                    return true;
                }
            }
            return false;

        }

        
    }
}
