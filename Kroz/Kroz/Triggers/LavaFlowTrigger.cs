using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Triggers
{
    class LavaFlowTrigger : BaseTrigger
    {
        String m_Type = "lava trigger";
        public LavaFlowTrigger(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        {
        }
        
        public override void OnCollision(Player player)
        {
            if (OnCollisionEvent != null)
                this.OnCollisionEvent(m_Type);
        }
         
        //public event EventHandler ItemEvent;
        public delegate void LavaFlow(String type);
        public event LavaFlow OnCollisionEvent;

        
    }
}
