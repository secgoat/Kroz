using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Whip : BaseItem
    {
         public Whip(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = "A whip! this will come in handy!";
         }

        public override void OnCollision(Player player)
        {
            player.Whips = 1;
            player.Score = 5;
        }

       
    }
}
