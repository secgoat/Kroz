﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Mobs
{
    class WallMob : BaseMob
    {
         public WallMob(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, 1, 5)
        {

        }
    }
}
