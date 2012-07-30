using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Kroz.Items;

namespace Kroz.Triggers
{
    class Earthquake : BaseTrigger
    {
         Random rand = new Random();
        List<Wall> newWallLocations = new List<Wall>();
        String m_Type = "earthquake";

        public Earthquake(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        {
        
        }

        public override void OnCollision(Player player)
        {
            if (StartEarthquake != null)
                this.StartEarthquake(m_Type);
        }

        public override void OnCollision(LevelManager level)
        {
            if (StartEarthquake != null)
                this.StartEarthquake(m_Type);
        }

        //public event EventHandler ItemEvent;
        public delegate void QuakeEvent(String type);
        public event QuakeEvent StartEarthquake;

    }
}
