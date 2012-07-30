using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Mobs
{
    public class Lava : BaseItem
    {
        bool flowing = true;
        int m_Strength;
        int m_TimeSinceLastFlow = 0;
        public Lava(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed, int scoreValue, int millisecondsPerFrame) 
            : base(textureImage, position, frameSize, collisionOffset, speed, scoreValue, millisecondsPerFrame)
        {
            this.m_Strength = 10;
        }

        public int TimeSinceLastFlow { get { return m_TimeSinceLastFlow; } set { m_TimeSinceLastFlow += value; } }
        public bool Flowing { get { return flowing; } set { flowing = value; } }

        public delegate void Flow(Lava lava);
        public event Flow LavaFlowEvent;

  public override void  OnCollision(Player player)
        {
            player.Health = -m_Strength;
 	         base.OnCollision(player);
        }  

        public void Update(GameTime gameTime)
        {
            if (this.flowing)
            {
               this.m_TimeSinceLastFlow += gameTime.ElapsedGameTime.Milliseconds;
                if (this.m_TimeSinceLastFlow > this.millisecondsPerFrame)
                {
                    this.m_TimeSinceLastFlow -= this.millisecondsPerFrame;
                    if (this.LavaFlowEvent != null)
                        this.LavaFlowEvent(this);
                }
            }
            base.Update(gameTime);
        }
    }
}
