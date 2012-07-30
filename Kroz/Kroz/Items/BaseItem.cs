using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz
{
    public abstract class BaseItem : Sprite
    {
        protected String m_Description; //use this to describe the item, a message will be printed to the console with this info on collison
        protected String m_Type; //use this for feeding to events? 
        public String Description { get { return m_Description; } }
        public String Type { get { return m_Type; } }

     /*  public override void OnCollision(Player player)
        {
            if (OnCollisionEvent != null)
                this.OnCollisionEvent(m_Type);
        }

        public override void OnCollision(LevelManager level)
        {
            if (OnCollisionEvent != null)
                this.OnCollisionEvent(m_Type);
        }

        //public event EventHandler ItemEvent;
        public delegate void ItemEvent(String type);
        public event ItemEvent OnCollisionEvent;

        protected virtual void EventOnCollision()
        {
            if(OnCollisionEvent != null)
                this.OnCollisionEvent(m_Type);
        } */
      
        public BaseItem(Texture2D textureImage, Vector2 position, Point frameSize,
             int collisionOffset, Vector2 speed, int scoreValue, int millisecondsPerFrame)
             : base(textureImage, position, frameSize, collisionOffset, speed, scoreValue, millisecondsPerFrame)
        {}
         

    }
}
