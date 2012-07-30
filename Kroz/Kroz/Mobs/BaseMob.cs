using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Kroz.Util;
using Kroz.Items;

namespace Kroz
{
    public class BaseMob : Sprite
    {
        public int m_Strength;
        double m_Sensitivity; //use this to tell how close the player has to be fore movement
        
        //Static variables to change spped of mobs on map / freeze;
        static bool m_fast;
        static int m_fastTimer = 10000;
        static int m_timeSinceFast = 0;
        static bool m_slowed;
        static  int m_slowedTimer = 10000;
        static int m_TimeSinceSlowed = 0;
        static bool m_frozen;
        static  int m_FrozenTimer = 10000;
        static int m_TimeSinceFrozen = 0;


        public static bool Frozen { get { return m_frozen; } set { m_frozen = value; } }

        public Vector2 SetPosition { get { return position; } set { position = value; } }
        
        public BaseMob(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Vector2 speed,
            int scoreValue, int millisecondsPerFrame, int strength, int sensitivity)
            : base(textureImage, position, frameSize, collisionOffset, speed,
                scoreValue, millisecondsPerFrame)
        {
            m_Strength = strength;
            m_Sensitivity = sensitivity;
        }


        public static void EventHandler(object sender)
        {
            if (sender is FreezeMobs)
            {
                BaseMob.m_frozen = true;
            }
            if (sender is SlowMobs)
            {
                BaseMob.m_slowed = true;
            }
        }
        
        public double CheckDistanceToPlayer(Player player)
        {
            double distance;
            /*  this gets the distance between 2 points. absolute value so it is a positive number, pow raises first part 
             *  by second part Ie X to the power of 2. and then divide by IMGsize to find how many squares on the map away the 
             *  player is from the mob.
             */
            distance = Math.Abs(Math.Sqrt(Math.Pow((player.collisionRect.Center.X - this.collisionRect.Center.X), 2) + Math.Pow((player.collisionRect.Center.Y - this.collisionRect.Center.Y),2))) / frameSize.X;
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
                    dx = -1;
                    dy = 0;
                }
                if (degree == 45)
                {
                    dx = -1;
                    dy = -1;
                }
                if (degree == 90)
                {
                    dx = 0;
                    dy = -1;
                }
                if (degree == 135)
                {
                    dx = 1;
                    dy = -1;
                }
                if (degree == 180)
                {
                    dx = 1;
                    dy = 0;
                }
                if (degree == 225)
                {
                    dx = 1;
                    dy = 1;
                }
                if (degree == 270)
                {
                    dx = 0;
                    dy = 1;
                }
            }
            direction.X = dx;
            direction.Y = dy;
            return direction;
        }

        public override void OnCollision(Player player)
        {
            player.Health = -m_Strength;
            player.Score = scoreValue;
        }

        public virtual void Update(GameTime gameTime, Vector2 newPOS, LevelManager level)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            int affectedMilliseconds;
            /* use affectedMilliseconds to change the milliseconds per frame (or speed of the mob)
             * when something like a slow spell, or a fast spell is cast.
             */
            if (m_slowed)
                affectedMilliseconds = millisecondsPerFrame * 2;
            else if (m_fast)
                affectedMilliseconds = millisecondsPerFrame / 2;
            else
                affectedMilliseconds = millisecondsPerFrame;

            if (timeSinceLastFrame > affectedMilliseconds) 
            {
                timeSinceLastFrame -= affectedMilliseconds;
                SetPosition = newPOS;
                   
                if(BaseMob.m_frozen)
                    m_TimeSinceFrozen += gameTime.ElapsedGameTime.Milliseconds;
                if(BaseMob.m_frozen && (BaseMob.m_TimeSinceFrozen > BaseMob.m_FrozenTimer) )
                {
                    BaseMob.m_frozen = false;
                    BaseMob.m_TimeSinceFrozen = 0;
                }
                if (BaseMob.m_slowed)
                {
                    m_TimeSinceSlowed += gameTime.ElapsedGameTime.Milliseconds;
                }
                if (m_TimeSinceSlowed > m_slowedTimer)
                {
                    BaseMob.m_slowed = false;
                    m_TimeSinceSlowed = 0;
                }
            }
            base.Update(gameTime);
        }

    }
}
