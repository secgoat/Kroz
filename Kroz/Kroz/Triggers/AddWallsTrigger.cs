using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Kroz.Items;

namespace Kroz.Triggers
{
    
    class AddWallsTrigger : BaseTrigger
    {
        char trigger;
        List<Wall> m_HiddenWalls = new List<Wall>();
        public AddWallsTrigger(Texture2D imageTexture, Vector2 position, Point frameSize, char trigger)
            : base(imageTexture, position, frameSize)
        {  
            this.trigger = trigger;
        }

        public char Trigger { get { return trigger; } }
        public Wall AddWall { set { m_HiddenWalls.Add(value); } }
        
       
    }
}
