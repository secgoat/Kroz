using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz
{
    public class Statue : Wall
    {

        
        public Statue(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame, true, true, Color.SlateGray)
        {
            this.color = Color.SlateGray;
        }

        public delegate void StatueEvent(object sender);
        public event StatueEvent CheckPlayerDistanceEvent;

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

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame >= millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                if (this.CheckPlayerDistanceEvent != null)
                    this.CheckPlayerDistanceEvent(this);
            }
            base.Update(gameTime);
        }
       
    }
}
