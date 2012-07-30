using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class WhipAnimation : Sprite
    {
        private int millisecondsSinceLastCheck = 0;
        private bool whipping;
        private Point currentFrame;
        private Point sheetSize;
        int currentLoop;
        int strength; //use this to determine how likely the whip is to break a wall, statue etc. max strength 0f 3 = 75%;

         public WhipAnimation(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            this.whipping = false;
            this.currentFrame = new Point(0, 0);
            this.sheetSize = new Point(4, 0); //set this so it loops through twice
            this.currentLoop = 0;
            this.strength = 1;
             
        }


         public int MillisecondsSinceLastCheck { get { return millisecondsSinceLastCheck; } set { millisecondsSinceLastCheck += value; } }
        public bool Whip { set { whipping = true; } get { return whipping; } }

        public int  Strength 
        { 
            get { return strength; } 
            set 
            { 
                if( strength <= 4)
                    strength += value; 
            } 
        }

        public void EventHandler(object sender)
        {
            if (sender is WhipPowerRing)
            {
                if (strength <= 4)
                    strength += 1;
            }

        }

        public void Update(GameTime gameTime, Player player)
        {
            position = Vector2.Zero; 
            if (whipping)
            {
                position = player.GetPosition; 
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                { 
                    timeSinceLastFrame -= millisecondsPerFrame;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        currentLoop += 1;
                        if (currentLoop > 1)
                        {
                            currentLoop = 0;
                            whipping = false;
                        }
                    }
                }
                if (currentFrame.X == 0 && currentLoop == 0)
                {
                    position.Y -= 1;
                }
                if (currentFrame.X == 1 && currentLoop == 0)
                {
                    position.X -= 1;
                    position.Y -= 1;
                }
                if (currentFrame.X == 2 && currentLoop == 0)
                {
                    position.X -= 1;

                }
                if (currentFrame.X == 3 && currentLoop == 0)
                {
                    position.Y += 1;
                    position.X -= 1;
                }
                if (currentFrame.X == 0 && currentLoop == 1)
                {
                    position.Y += 1;
                }
                if (currentFrame.X == 1 && currentLoop == 1)
                {
                    position.X += 1;
                    position.Y += 1;
                }
                if (currentFrame.X == 2 && currentLoop == 1)
                {
                    position.X += 1;
                }
                if (currentFrame.X == 3 && currentLoop == 1)
                {
                    position.X += 1;
                    position.Y -= 1;
                }
            }
            //spriteRect = new Rectangle((int)position.X, (int)position.Y, frameSize.X, frameSize.Y);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (whipping)
            {
                spriteBatch.Draw(textureImage, spriteRect,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White);
            }
        }//end Draw
       
    }
}
