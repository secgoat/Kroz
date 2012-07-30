using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Mobs
{
    class MobileWall : BaseMob
    {
        bool canMove;

        public MobileWall(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, 2, 5)
        { }

        public override void OnCollision(Player player)
        {
            base.OnCollision(player);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds, Vector2 newPOS, LevelManager level)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                if (!level.CheckCollisions(newPOS, false))
                    SetPosition = newPOS;              
            }
            base.Update(gameTime, clientBounds);
        }
    }
}
