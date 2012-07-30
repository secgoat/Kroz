using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Kroz.Items;

namespace Kroz
{
    public class Player : Sprite
    {

        /* Members from Sprite Class
         * 
         * Texture2D textureImage;
         * protected Point frameSize;
         * int collisionOffset;
         * protected Vector2 speed;
         * protected Vector2 position;
         */
        //position variables used for collision detection
       // private Vector2 oldPOS, newPOS;

        //Game related variables
        int playerHealth;
        int playerWhips;
        int playerTeleports;
        int playerKeys;
        int playerScore;


        int visibilityTimer = 10000;
        int timeSinceInvis = 0;

        public Player(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame) : base(textureImage, position, frameSize, collisionOffset, speed,
            scoreValue, millisecondsPerFrame)
        {
            this.playerHealth = 20;
            this.playerWhips = 5;
            this.playerTeleports = 5;
            this.playerKeys = 50;
            this.playerScore = 0;
            this.m_Visible = true;
            //this.oldPOS = new Vector2(0, 0);
            //this.newPOS = new Vector2(0, 0);


        }

        public int Health { get { return playerHealth; }  set { playerHealth += value; } }
        public int Whips  { get { return playerWhips; }   set { playerWhips += value;}}
        public int Keys { get { return playerKeys; }    set { playerKeys += value; } }
        public int Teleports { get { return playerTeleports; } set { playerTeleports += value; } }
        public int Score { get { return playerScore; }   set { playerScore += value; } }
        public Vector2 SetPosition 
        { 
            get { return position; } 
            set { position = value; } 
        }
                
        public override void Update(GameTime gameTime)
        {
            spriteRect = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            if (!Visible)
            {
                timeSinceInvis += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceInvis > visibilityTimer) //if player hits an invis this will set them back to visible after said time
                {
                    this.Visible = true;
                    timeSinceInvis = 0;
                }
            }
            //stop sprite from leaving screen
            /*
             * not needed naymore since wall collison is in place and walls surround each level.
             * but leaving it here for posterity, and also i am kind of a jack ass some times and
             * forget how to do stuff
             * 
             * if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;*/
            base.Update(gameTime);
        }

       
    
    }
}
