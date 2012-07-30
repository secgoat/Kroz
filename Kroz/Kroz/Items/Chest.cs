using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Chest : BaseItem
    {
        Random randNum;
        
         public Chest(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = "You open an old chest to find ";
            m_Type = "chest";
            randNum = new Random();
         }

        public override void OnCollision(Player player)
        {
            int gems = randNum.Next(1, 5);
            int whips = randNum.Next(1, 5);
            player.Whips = whips;
            player.Health = gems;
            player.Score = (5 * gems) + (5 * whips);
            m_Description += gems + " Gems and " + whips + " Whips";
        }

       
    }
}
