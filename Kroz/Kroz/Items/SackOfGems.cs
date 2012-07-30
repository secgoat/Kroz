using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class SackOfGems : BaseItem
    {
        public SackOfGems(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {  
            this.m_Description = " You find an sack filled with gems";
        }

        public override void OnCollision(Player player)
        {
            player.Health = 25; //add 25 to players health using Health
            player.Score = 125;
        }
    }
}
