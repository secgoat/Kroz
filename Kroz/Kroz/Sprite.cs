using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz
{

    public abstract class Sprite
    {
        protected Texture2D textureImage;
        protected Point frameSize;
        int collisionOffset;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame = 500;
        protected Vector2 speed;
        protected Vector2 position;
        protected Rectangle spriteRect;
        protected Color color;
        protected bool m_Visible;

        public int scoreValue { get; protected set; }
        public bool Visible { get { return m_Visible; } set { m_Visible = value; } }
        
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Vector2 speed, int scorevalue, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.speed = speed;
            this.scoreValue = scoreValue;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.spriteRect = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y); //use this for scaling the sprites to another size right now 16x16
            this.color = Color.White;
            this.m_Visible = true;
        }// end Sprite constructor

        public virtual void Update(GameTime gameTime)
        {
            //use this to calculate sprite x/y in pixels instead if in array coords
            int x = (int)(position.X * frameSize.X);
            int y = (int)(position.Y * frameSize.Y);
            spriteRect = new Rectangle(x, y, frameSize.X, frameSize.Y);
            
        } // end Update

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(m_Visible)
                spriteBatch.Draw(textureImage, spriteRect, color);
        }//end Draw

        //properties

        public Vector2 GetPosition { get { return position; } }

        public int MillisecondsPerFrame { get { return millisecondsPerFrame; } }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }//end get
        }//end collisionRect get

        public delegate void CollisionEvent(object sender);
        public event CollisionEvent OnCollisionEvent;
        
        public virtual void OnCollision(Player player) //this will do nothing if no event is attached, however will fire event if defined in levelManager
        {
            if (this.OnCollisionEvent != null)
                this.OnCollisionEvent(this);
        }

        public virtual void OnCollision(LevelManager level)
        {
            if (this.OnCollisionEvent != null)
                this.OnCollisionEvent(this);
        }


        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
                position.X > clientRect.Width ||
                position.Y < -frameSize.Y ||
                position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }//end IsOutOfBounds

    }//end Class Sprite
}//end nameSpace
