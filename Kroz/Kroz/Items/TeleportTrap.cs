using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class TeleportTrap : BaseItem
    {
        Random rand = new Random();
        public List<Vector2> SetEmptySpaces { set { m_EmptySpaces = value; } }
        protected List<Vector2> m_EmptySpaces;
        
        public TeleportTrap(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_EmptySpaces = new List<Vector2>();
            this.m_Type = "Teleport Trap";
            this.m_Description = "You step down down and feel disoriented;";

        
        }
        
        public override void OnCollision(Player player)
        {
          int randomNum = rand.Next(0, m_EmptySpaces.Count);
          player.SetPosition = m_EmptySpaces[randomNum];
            
        }
    }
}
