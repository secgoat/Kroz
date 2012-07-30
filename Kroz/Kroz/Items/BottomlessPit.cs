using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class BottomlessPit : BaseItem
    {
        public BottomlessPit(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed, int scoreValue, int millisecondsPerFrame) 
            : base(textureImage, position, frameSize, collisionOffset, speed, scoreValue, millisecondsPerFrame)
        {
            this.m_Description = "You step on a bottomless pit and begin to fall!";
            this.m_Type = "pit";
        }

        public delegate void PitFall();
        public PitFall PitCollision;

        public override void OnCollision(LevelManager level)
        {
            if (PitCollision != null)
                this.PitCollision();
        }
    }
}
