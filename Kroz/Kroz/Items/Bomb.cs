using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Bomb : BaseItem
    {
         public Bomb(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = "You set of a rather large explosion!";
            m_Type = "bomb";
         }

        /* public delegate void Explosion(Bomb bomb);
         public event Explosion ExplosionEvent;

         public override void OnCollision(Player player)
         {
             if (ExplosionEvent != null)
                 this.ExplosionEvent(this);
         }

         public override void OnCollision(LevelManager level)
         {
             if (ExplosionEvent != null)
                 this.ExplosionEvent(this);
         }*/

    }
}
