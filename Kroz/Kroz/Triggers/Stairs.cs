using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Triggers
{
     class Stairs : BaseTrigger
    {
         public BaseTrigger.FiredEvent OnStairs;

         public Stairs(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        { }
        
         public override void OnCollision(Player player)
        {
            if (OnStairs != null)
                OnStairs(this);
        }
        
         public override void OnCollision(LevelManager level)
        {  
            base.OnCollision(level);
        }
         

    }
}
