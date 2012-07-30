using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Gem : BaseItem
    {
         public Gem(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = " You pick up a gem";
         }

        public override void OnCollision(Player player)
        {
            player.Health = 1; //add 1 to players health using Health
            player.Score = 5; 
        }

       
    }
}
