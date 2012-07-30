using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Triggers
{
    class ActivateMobileWallTrigger : BaseTrigger
    {
        String m_Type = "activate wall trigger";

        public ActivateMobileWallTrigger(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        {
        }

        public override void OnCollision(Player player)
        {
            if (OnCollisionEvent != null)
                this.OnCollisionEvent(m_Type);
        }

        public delegate void ActivateWalls(String type);
        public event ActivateWalls OnCollisionEvent;
    }
}
