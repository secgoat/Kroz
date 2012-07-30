using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class Teleporter : BaseItem
    {
        public Teleporter(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
           int scoreValue, int millisecondsPerFrame, Color color)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            this.color = color;
        }
    }
}
