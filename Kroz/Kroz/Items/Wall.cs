using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz
{
    public class Wall : BaseItem
    {
        private bool m_Breakable;
        //private bool m_Visible;
        
        public Wall(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame, bool breakable, bool visible, Color color)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Breakable = breakable;
            this.m_Visible = visible;
            this.color = color;
        }
        public bool Breakable { get { return m_Breakable; } }
        public override void OnCollision(Player player)
        {
            if (!m_Visible)
                m_Visible = true;

            base.OnCollision(player);
        }

        
    }
}
