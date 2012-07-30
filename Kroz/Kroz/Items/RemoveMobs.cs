using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class RemoveMobs : BaseItem
    {
         public RemoveMobs(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {  
            this.m_Description = "You Set Off an electric storm which obliterates the enemies around you!";
            this.m_Type = "less mobs";
        }

        
    }
}
