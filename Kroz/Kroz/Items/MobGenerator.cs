using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class MobGenerator : Wall
    {
        int millisecondsSinceLastMob = 0;

         public MobGenerator(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, true, true, Color.White)
        {  
            this.m_Description = "This object appears to be creating more monsters!";
            this.m_Type = "mob generator";
        }

         public delegate void Generate();
         public event Generate GenerateMobEvent;

         public override void Update(GameTime gameTime)
         {
             millisecondsSinceLastMob += gameTime.ElapsedGameTime.Milliseconds;
             if (millisecondsSinceLastMob > millisecondsPerFrame)
             {
                 millisecondsSinceLastMob -= millisecondsPerFrame;
                 if (GenerateMobEvent != null)
                     this.GenerateMobEvent();
             }

             base.Update(gameTime);
         }
    }
}
