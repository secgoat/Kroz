using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Kroz.Items;

namespace Kroz.Triggers
{
    class HideWalls : BaseTrigger
    {

        String m_Type = "hide walls";

        public HideWalls(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        {
        
        }

       /* public override void OnCollision(Player player)
        {
            if (StartHideWalls != null)
                this.StartHideWalls(m_Type);
        }

        public override void OnCollision(LevelManager level)
        {
            if (StartHideWalls != null)
                this.StartHideWalls(m_Type);
        }

        //public event EventHandler ItemEvent;
        public delegate void HideWallsEvent(String type);
        public event HideWallsEvent StartHideWalls; */

    }
}
