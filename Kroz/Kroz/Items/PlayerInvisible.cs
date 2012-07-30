using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    public class PlayerInvisible : BaseItem
    {
        public PlayerInvisible(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
           int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            this.m_Type = "player invis";
            this.m_Description = "You pick up a strange item. You can no longer see yourself!";
        }

        public override void OnCollision(Player player)
        {
            player.Visible = false;
            base.OnCollision(player);
        }
    }
}
