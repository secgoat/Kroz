using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Items
{
    class MobileWall : Wall
    {
       static bool canMove;
       int m_Sensitivity;

        public MobileWall(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, true, true, Color.Beige)
        {
            MobileWall.canMove = false;
            this.m_Sensitivity = 5;
        }
        public Vector2 SetPosition { get { return position; } set { position = value; } }
        public double CheckDistanceToPlayer(Player player)
        {
            double distance;
            /*  this gets the distance between 2 points. absolute value so it is a positive number, pow raises first part 
             *  by second part Ie X to the power of 2. and then divide by IMGsize to find how many squares on the map away the 
             *  player is from the mob.
             */
            distance = Math.Abs(Math.Sqrt(Math.Pow((player.collisionRect.Center.X - this.collisionRect.Center.X), 2) + Math.Pow((player.collisionRect.Center.Y - this.collisionRect.Center.Y), 2))) / frameSize.X;
            return distance;
        }
        public Vector2 DirToPlayer(Player player)
        {
            Vector2 direction;
            double radian;
            double degree;
            int dx = 0;
            int dy = 0;

            if (this.CheckDistanceToPlayer(player) <= this.m_Sensitivity)
            {
                //get the radian to the player
                radian = Math.Atan2(this.collisionRect.Center.Y - player.collisionRect.Center.Y, this.collisionRect.Center.X - player.collisionRect.Center.X);
                //convert radian to degrees to tell mob which direction to move
                degree = Util.Calculate.RadianToDegree(radian);
                if (degree < 0) //always get a positive degree
                    degree += 360;
                if (degree == 0)
                {
                    dx = -frameSize.X;
                    dy = 0;
                }
                if (degree == 45)
                {
                    dx = -frameSize.X;
                    dy = -frameSize.Y;
                }
                if (degree == 90)
                {
                    dx = 0;
                    dy = -frameSize.Y;
                }
                if (degree == 135)
                {
                    dx = frameSize.X;
                    dy = -frameSize.Y;
                }
                if (degree == 180)
                {
                    dx = frameSize.X;
                    dy = 0;
                }
                if (degree == 225)
                {
                    dx = frameSize.X;
                    dy = frameSize.Y;
                }
                if (degree == 270)
                {
                    dx = 0;
                    dy = frameSize.Y;
                }
            }
            direction.X = dx;
            direction.Y = dy;
            return direction;
        }
        public override void OnCollision(Player player)
        {
            base.OnCollision(player);
        }

        static public void ActivateEventHandler(string type)
        {
            MobileWall.canMove = true;
        }

        public void Update(GameTime gameTime, Vector2 newPOS, LevelManager level)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                if (!level.CheckCollisions(newPOS, false) && MobileWall.canMove)
                    SetPosition = newPOS;              
            }
            base.Update(gameTime);
        }
    }
}
