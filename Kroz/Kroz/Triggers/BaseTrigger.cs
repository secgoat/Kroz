using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Triggers
{
    abstract class BaseTrigger : Sprite
    {
        //protected bool hasImage;
        //protected Texture2D image;
        //Vector2 m_Position;
        protected List<Vector2> m_EmptySpaces;
        //Point m_FrameSize;
        protected List<Vector2> m_Positions;

        public delegate void FiredEvent(object sender);

        public BaseTrigger(Texture2D imageTexture, Vector2 position, Point frameSize) : 
            base(imageTexture, position, frameSize, 0, Vector2.Zero, 0, 0)
        {
            //image = imageTexture;
            //m_Position = position;
            //m_FrameSize = frameSize;
            m_EmptySpaces = new List<Vector2>();
            m_Positions = new List<Vector2>();
        }

        public List<Vector2> SetEmptySpaces { set { m_EmptySpaces = value; } }
        
    }
}
