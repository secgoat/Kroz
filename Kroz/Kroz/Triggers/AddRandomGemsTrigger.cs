using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Kroz.Items;

namespace Kroz.Triggers
{
    class AddRandomGemsTrigger : BaseTrigger
    {
        Random rand = new Random();
        List<Vector2> newGemLocations = new List<Vector2>();

        public AddRandomGemsTrigger(Texture2D imageTexture, Vector2 position, Point frameSize)
            : base(imageTexture, position, frameSize)
        {  }



       
    }
}
