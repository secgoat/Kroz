using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class FreezeMobs : BaseItem
    {
        public FreezeMobs(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
           int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            this.m_Type = "freeze mobs";
            this.m_Description = "You pick up a cold orb, monsters around you seem to freeze in place.";
        }

        //public delegate void FreezeEvent(String type);
        //public event FreezeEvent OnCollisionEvent;
        
        /*public override void OnCollision(Player player)
        {
            if(OnCollisionEvent != null)
                OnCollisionEvent(m_Type);
        } */

    } 
}
