using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class KrozLetter : BaseItem
    {
        char m_Letter;
        static List<char> lettersPickedUp = new List<char>();
         public KrozLetter(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame, char letter)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Description = "you pick up an ancient rune which glows with power.";
            m_Letter = letter;
         }

        public override void OnCollision(Player player)
        {
            lettersPickedUp.Add(this.m_Letter);

            if(lettersPickedUp.Count == 4 && 
                (lettersPickedUp[0] == 'k' && lettersPickedUp[1] == 'r' && lettersPickedUp[2] == 'o' && lettersPickedUp [3] == 'z'))
            {
                player.Score = 10000;
            }
            
        }

       
    }
}
