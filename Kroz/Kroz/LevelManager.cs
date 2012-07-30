using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Kroz.Items;
using Kroz.Triggers;
using Kroz.Mobs;
using Kroz.Menu;
using Kroz.Util;

namespace Kroz
{
    public class LevelManager : GameScreen
    {
        #region variables
        //-----FOR FPS COUNTER---------------------------------------------------
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        //----------------------------------------
        Game game;
        public static Point spriteSize; //in pixels each direction
        SpriteFont baseFont;
        Rectangle imageRectangle;
        //--------------Input from keyboard----------------------//
        KeyboardState keyboardState;
        KeyboardState prevKeyboardState;
        //--------------player--------------------------------//
        Player player;
        Vector2 playerPOS = Vector2.Zero;
        Vector2 playerOldPOS = Vector2.Zero;
        Vector2 playerNewPOS = Vector2.Zero;
        WhipAnimation playerWhip;
        //------------------textures for walls etc-------------//
        #region textures
        Texture2D ancientTabletTexture;
        Texture2D breakableTexture;
        Texture2D bombTexture;
        Texture2D chestTexture;
        Texture2D doorTexture;
        Texture2D floorTexture;
        Texture2D freezeTexture;
        Texture2D gemTexture;
        Texture2D goldTexture;
        Texture2D invisTexture;
        Texture2D kTexture;
        Texture2D keyTexture;
        Texture2D lavaTexture;
        Texture2D mobGeneratortexture;
        Texture2D oTexture;
        Texture2D pitTexture;
        Texture2D playerTexture;
        Texture2D powerRingTexture;
        Texture2D rTexture;
        Texture2D removeMobTexture;
        Texture2D removeWallsTexture;
        Texture2D sackTexture;
        Texture2D stairsTexture;
        Texture2D slowMobTexture;
        Texture2D teleportTexture;
        Texture2D teleporterTexture;
        Texture2D teleportTrapTexture;
        Texture2D treeWallTexture;
        Texture2D transparentTexture;
        Texture2D wallTexture;
        Texture2D waterTexture;
        Texture2D whipAnimation;
        Texture2D whipTexture;
        Texture2D zTexture;
        //MOb textures
        Texture2D gnomeTexture;
        Texture2D elfMummyTexture;
        Texture2D ogreLordTexture;
        Texture2D umberHulkTexture;
        //COLORS
        Color brown = Color.SandyBrown;
        Color white = Color.White;
        Color gray = Color.Gray;
        #endregion
        //---------keep track of level number and the current map--------------//
        private int level;
        private List<string> levelMap;
        private int mapWidth; 
        private int mapHeight;
      
        List<Sprite> levelObjects = new List<Sprite>(); //this will hold everything eventually.
        Dictionary<char, List<Wall>> wallsToTrigger = new Dictionary<char, List<Wall>>()
        {//use this key will be '4','5','6'.'Y' Data will be positions of these walls to remove
            {'4', new List<Wall>()}, //remove walls
            {'5', new List<Wall>()}, //remove walls
            {'6', new List<Wall>()}, //remove walls
            {'y', new List<Wall>()}, //remove walls
            {'7', new List<Wall>()}, //add walls
            {'8', new List<Wall>()}, //add walls
            {'9', new List<Wall>()} //add walls
        };

        List<Sprite>[,] levelLocations = new List<Sprite>[26,67];
        //--------------Random number genrartor for good measure-------------------------------------------//
        Random rand = new Random();
        //--------------VARIABLES TO KEEP TRACK OF MESSAGES-----------------------------------------------//
        List<String> levelMessages = new List<String>();
        #endregion

        public int Level 
        { 
            get { return level; } 
            set 
            { 
                level = value;
                this.ClearLevel();
                this.LoadLevelMap();
            } 
        }

        public List<Vector2> AddGems
        { // get a list of 3 ints: number of new gems , x pos and y pos
            set 
            {
                for (int i = 1; i <= value[0].X; i++)
                {
                    levelObjects.Add(new Gem(gemTexture, new Vector2(value[i].X, value[i].Y), spriteSize, 0, Vector2.Zero, 5, 0));
                }
            } 
        }

        public List<Wall> AddWalls{ set { levelObjects.AddRange(value); } }
        
        public List<Wall> RemoveWalls
        {
            set
            { /*iterate through levelWalls and match Positions to remove walls*/
                for (int i = 0; i < value.Count; ++i)
                {
                    Wall wall1 = value[i];
                    for (int j = 0; j < levelObjects.Count; ++j)
                    {
                        if (levelObjects[j].GetType().Name == "Wall")
                        {

                            Wall wall2 = (Wall)levelObjects[j];
                            if (wall1.collisionRect.Intersects(wall2.collisionRect))
                            {
                                levelObjects.RemoveAt(j);
                                --j;
                            }
                        }
                    }
                }
            }
        }

        public LevelManager(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            level = 1;
            levelMap = new List<string>();
            spriteSize = new Point(16, 16); //set the sprite size here
            
            int bound0 = levelLocations.GetUpperBound(0);
            int bound1 = levelLocations.GetUpperBound(1);
            for(int y = 0; y <= bound0; y++)
            {
                for(int x = 0; x <= bound1; x++)
                {
                    levelLocations[y,x] = new List<Sprite>();
                }
            }
            this.LoadContent();
            this.LoadLevelMap();
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            baseFont = Game.Content.Load<SpriteFont>("BaseFont");
            //-------------LOAD TEXTURES------------------------------//
            ancientTabletTexture = Game.Content.Load<Texture2D>(@"Images\tablet");
            breakableTexture = Game.Content.Load <Texture2D>(@"Images\breakable");
            bombTexture = Game.Content.Load<Texture2D>(@"Images\bomb");
            chestTexture =  Game.Content.Load<Texture2D>(@"Images\chest");
            doorTexture =   Game.Content.Load<Texture2D>(@"Images\door");
            elfMummyTexture = Game.Content.Load<Texture2D>(@"Images\elf_mummy");
            floorTexture =  Game.Content.Load<Texture2D>(@"Images\floor");
            freezeTexture = Game.Content.Load<Texture2D>(@"Images\freeze_monester");
            gemTexture =    Game.Content.Load<Texture2D>(@"Images\gem");
            gnomeTexture = Game.Content.Load<Texture2D>(@"Images\gnome");
            goldTexture =   Game.Content.Load<Texture2D>(@"Images\gold");
            invisTexture = Game.Content.Load<Texture2D>(@"Images\invisibility");
            kTexture = Game.Content.Load<Texture2D>(@"Images\k");
            keyTexture = Game.Content.Load<Texture2D>(@"Images\key");
            lavaTexture = Game.Content.Load<Texture2D>(@"Images\lava");
            mobGeneratortexture = Game.Content.Load<Texture2D>(@"Images\MobGenerator");
            oTexture = Game.Content.Load<Texture2D>(@"Images\o");
            ogreLordTexture = Game.Content.Load<Texture2D>(@"Images\ogre_lord");
            playerTexture = Game.Content.Load<Texture2D>(@"Images\player");
            pitTexture = Game.Content.Load<Texture2D>(@"Images\pit");
            powerRingTexture = Game.Content.Load<Texture2D>(@"Images\ring");
            rTexture = Game.Content.Load<Texture2D>(@"Images\r");
            removeMobTexture = Game.Content.Load<Texture2D>(@"Images\zap_monster");
            removeWallsTexture = Game.Content.Load<Texture2D>(@"Images\removewalls");
            sackTexture = Game.Content.Load<Texture2D>(@"Images\sack");
            stairsTexture = Game.Content.Load<Texture2D>(@"Images\stairs");
            slowMobTexture = Game.Content.Load<Texture2D>(@"Images\slow_monster");
            teleportTexture = Game.Content.Load<Texture2D>(@"Images\teleport");
            teleporterTexture = Game.Content.Load<Texture2D>(@"Images\teleporter");
            teleportTrapTexture = Game.Content.Load<Texture2D>(@"Images\tele_trap");
            treeWallTexture = game.Content.Load<Texture2D>(@"Images\tree");
            transparentTexture = Game.Content.Load<Texture2D>(@"transparent");
            umberHulkTexture = Game.Content.Load<Texture2D>(@"Images\umber_hulk");
            wallTexture =   Game.Content.Load<Texture2D>(@"Images\wall");
            waterTexture =  Game.Content.Load<Texture2D>(@"Images\water");
            whipAnimation = Game.Content.Load<Texture2D>(@"Images\whipAnimation16"); //drop the 16 when you got to 32x32 tiles
            whipTexture =   Game.Content.Load<Texture2D>(@"Images\whip");
            zTexture = Game.Content.Load<Texture2D>(@"Images\z");
            player = new Player(Game.Content.Load<Texture2D>(@"Images\player"), new Vector2(32,32),
                spriteSize, 2, Vector2.Zero, 0, 350);
            playerWhip = new WhipAnimation(whipAnimation, Vector2.Zero, spriteSize, 0, Vector2.Zero, 0, 350);
            base.LoadContent();
        }
        
        public override void Update(GameTime gameTime)
        {
            //use this next cod eblock to update the FPS counter.

            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            /*Use this to update the players movement and check for collision detecion
             *we keep it at this level in the LevelManager so that we can keep track off all sprites items etc 
             *and player / item doesnt have to keep track of world etc.
             */
            Vector2 inputDirection = Vector2.Zero;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left)) //move left
                inputDirection.X -= 1;
                //inputDirection.X -= spriteSize.X;
            if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right)) //move right
                inputDirection.X += 1;
                //inputDirection.X += spriteSize.X; 
            if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up)) //move up
                inputDirection.Y -= 1;
                //inputDirection.Y -= spriteSize.Y;
            if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down)) //move down
                inputDirection.Y += 1;
                //inputDirection.Y += spriteSize.Y;
            if (keyboardState.IsKeyDown(Keys.T) && prevKeyboardState.IsKeyUp(Keys.T)) // teleport
                Teleport();
            prevKeyboardState = keyboardState; //keep this here as I only care about prevKeyboard state on movement i think
            
            if (keyboardState.IsKeyDown(Keys.Space)) //whip
                playerWhip.Whip = true;
           
            if(keyboardState.IsKeyDown(Keys.Escape))//exit
                Game.Exit();

            playerNewPOS = player.GetPosition + inputDirection; //make the next move based onb player position and what key is pressed
            /* new attempt at updatign player by checking to see if collision with solid object is false
             * if(CheckCollision == false) //this means Check collision needs to return true if a wall, or other solid object is coolided with
             * //otherwise return false
             * //lenient colison could still work lenient = true means it only returns true if a solid is hit
             * //lenient = false would return true if anything is hit.
             *     player.SetPosition(playerNewPOS)
             * 
             * 
             * 
             */

            if (CheckCollisions(player, playerNewPOS) == false) //checks to make sure there is no collision between rigid items, which will return true
            {  
                Vector2 pos = player.GetPosition;
                int x = (int)pos.X;
                int y = (int)pos.Y;
                int newX  = (int)playerNewPOS.X;
                int newY  = (int)playerNewPOS.Y;
                levelLocations[y, x].Remove(player);
                levelLocations[newY, newX].Add(player);
                player.SetPosition = playerNewPOS;
                player.Update(gameTime);
            }
            

            playerWhip.Update(gameTime, player);
            playerWhip.MillisecondsSinceLastCheck = gameTime.ElapsedGameTime.Milliseconds; //use this to keep track of how often we check for collision
            if (playerWhip.MillisecondsSinceLastCheck >= playerWhip.MillisecondsPerFrame)
            {
                playerWhip.MillisecondsSinceLastCheck = -playerWhip.MillisecondsPerFrame;
                CheckCollisions(playerWhip, playerWhip.GetPosition);
            }

             //update mobs
            int yMax = levelLocations.GetUpperBound(0);
            int xMax = levelLocations.GetUpperBound(1);
            Vector2 playerPos = player.GetPosition;
            /*int yMin = (int)playerPos.Y - 2;
            if (yMin < 0)
                yMin = 0;
            int yMax = (int)playerPos.Y + 2;
            int xMin = (int)playerPOS.X - 2;
            if (xMin < 0)
                xMin = 0;
            int xMax = (int)playerPos.X + 2; */

            for (int y = 0; y <= yMax; y++)
            {
                for (int x = 0; x <= xMax; x++)
                {
                    for (int sprite = 0; sprite < levelLocations[y, x].Count; sprite++)
                    {
                        
                        if (levelLocations[y, x][sprite] is BaseMob)
                        {
                            Vector2 dir = Vector2.Zero;
                            Vector2 newMobPos = Vector2.Zero;
                             BaseMob mob = (BaseMob)levelLocations[y, x][sprite];
                            dir = mob.DirToPlayer(player);
                            newMobPos = mob.GetPosition + dir;
                            levelLocations[y, x][sprite].Update(gameTime);
                            if (inRange(new Point(x,y), new Point((int)playerPos.X, (int)playerPos.Y), Vector2.One) && 
                                /*(CheckCollisions(mob, newMobPos) == false) &&*/ (BaseMob.Frozen == false))
                            {
                                Vector2 pos = mob.GetPosition;
                                int mobX = (int)pos.X;
                                int mobY = (int)pos.Y;
                                int newMobX = (int)newMobPos.X;
                                int newMobY = (int)newMobPos.Y;
                                levelLocations[mobY, mobX].Remove(mob);
                                levelLocations[newMobY, newMobX].Add(mob);
                                mob.Update(gameTime, newMobPos, this);

                            }
                        }
                    }
                }
            }
        
           /* for (int i = 0; i < levelObjects.Count; i++)
            {
                Vector2 dir = Vector2.Zero;
                Vector2 newMobPos = Vector2.Zero;
                Sprite sprite = levelObjects[i];

                Type type = levelObjects[i].GetType();
                if (type.BaseType.Name == "BaseMob")
                {
                    BaseMob mob = (BaseMob)sprite;
                    dir = mob.DirToPlayer(player);
                    newMobPos = mob.GetPosition + dir;
                    if ((CheckCollisions(mob, newMobPos) == false) && (BaseMob.Frozen == false))
                    {
                        mob.Update(gameTime, newMobPos, this);
                    }
                   
                }
                if (type.Name == "MobileWall")
                {
                    MobileWall wall = (MobileWall)sprite;
                    dir = wall.DirToPlayer(player);
                    newMobPos = wall.GetPosition + dir;
                    wall.Update(gameTime, newMobPos, this);
                }
                //this should take care of 90% of all updates.
                levelObjects[i].Update(gameTime);
            } */
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)

        {
            //foreach (Sprite sprite in levelObjects) { sprite.Draw(gameTime, spriteBatch); }
            int yMax = levelLocations.GetUpperBound(0);
            int xMax = levelLocations.GetUpperBound(1);
            for (int y = 0; y <= yMax; y++)
            {
                for (int x = 0; x <= xMax; x++)
                {
                    for (int sprite = 0; sprite < levelLocations[y, x].Count; sprite++)
                    {
                        levelLocations[y, x][sprite].Draw(gameTime, spriteBatch);
                    }
                }
            }
           // player.Draw(gameTime, spriteBatch);

            //update FPS before drawing it
            frameCounter++;
            String fps = string.Format("FPS: {0}", frameRate);
            spriteBatch.DrawString(baseFont, fps, new Vector2(150, 0), Color.Black);
            spriteBatch.DrawString(baseFont, fps, new Vector2(150, 0), Color.White);

            //draw other messages now
            
            spriteBatch.DrawString(baseFont, "Gems:      " + player.Health, new Vector2(10, 400), Color.White);
            spriteBatch.DrawString(baseFont, "Whips:     " + player.Whips, new Vector2(10, 420), Color.White);
            spriteBatch.DrawString(baseFont, "Keys:      " + player.Keys, new Vector2(10, 440), Color.White);
            spriteBatch.DrawString(baseFont, "Teleports: " + player.Teleports, new Vector2(10, 460), Color.White);
            spriteBatch.DrawString(baseFont, "Score:     " + player.Score, new Vector2(10, 480), Color.White);
            
            spriteBatch.DrawString(baseFont, "Location: " + player.GetPosition, new Vector2(400, 480), Color.White);
            Vector2 array = player.GetPosition;
            //spriteBatch.DrawString(baseFont, "Array: " + levelLocations[(int)array.Y,(int)array.X].ToString(), new Vector2(175, 420), Color.White);
            for (int i = 0; i < levelLocations[(int)array.Y, (int)array.X].Count; i++)
            {
                spriteBatch.DrawString(baseFont, levelLocations[(int)array.Y, (int)array.X][i].ToString(), new Vector2(200, (i * 20) + 400), Color.White);
            }

            spriteBatch.DrawString(baseFont, "Level: " + level, new Vector2(0, 0), Color.White);
            levelMessages.Reverse();
            for (int i = 0; i < levelMessages.Count; i++)
            {
                spriteBatch.DrawString(baseFont, levelMessages[i], new Vector2(200, (i * 20) + 400), Color.White);
            }
            levelMessages.Reverse();
            playerWhip.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
            
        }
//----------------------My functions not XNA ones---------------------------------------//

        public List<Vector2> FindEmptySpaces(bool lenientCollision)
        {
            //use this to find empty spaces on the map for things like player teleport, teleport traps, and the gem triggers etc.
            //use bool lenientCollision = true if "empty spaces" can contain items or mobs.
            // use lenient collison = false if "empty spaces" can have nothign in it

            List<Vector2> emptySpaces = new List<Vector2>();
            Vector2 position;
            int mapX = 0;
            int mapY = 0;
            for (int y = 1; y < levelMap.Count() - 1; y++) // I think this should check everythign except the outside squares which wil always be walls hence the -1's
            {
                for (int x = 1; x < levelMap[y].Length - 1; x++)
                {
                    mapX = x * spriteSize.X;
                    mapY = y * spriteSize.Y;
                    position = new Vector2(mapX, mapY);

                    if (!CheckCollisions(position, lenientCollision))
                        emptySpaces.Add(position);
                }
            }
            return emptySpaces;
        }

        public bool CheckCollisions(Vector2 position, bool lenientCollision)
        {
            //use this for checking for blank spaces, IE no Spirte to check collisons for.
            Rectangle collisionRect = new Rectangle((int)position.X, (int)position.Y, spriteSize.X, spriteSize.Y);
            for (int i = 0; i < levelObjects.Count; i++)
            {
                
                Type objectType = levelObjects[i].GetType();
                if (objectType.Name == "Floor")
                    continue;
                //the following two would be lenientCollision == false
                if (lenientCollision == false)
                {
                    if (levelObjects[i].collisionRect.Intersects(collisionRect))
                        return true;

                    if (player.collisionRect.Intersects(collisionRect))
                        return true;
                }
                /* if lenient collsion == true
                 *  return true only if collided with walls, doors, water, stairs etc.
                 *  redoing all the items that would fit here to inherit from wall.
                 *  then can do if type.baseType.Name == "Wall" to check for dissallowed collison
                 */  
                if (objectType.BaseType.Name == "Wall" && levelObjects[i].collisionRect.Intersects(collisionRect))
                {
                    return true;
                }
               
            }
            return false;
        }

        
        public bool CheckCollisions(object collider, Vector2 POS)
        {
            Rectangle checkCollisionRect = new Rectangle((int)POS.X, (int)POS.Y, spriteSize.X, spriteSize.Y);
            int yMax = levelLocations.GetUpperBound(0);
            int xMax = levelLocations.GetUpperBound(1);
            for (int y = 0; y <= yMax; y++)
            {
                for (int x = 0; x <= xMax; x++)
                {
                    for (int sprite = 0; sprite < levelLocations[y, x].Count; sprite++)
                    {
                Type type = levelObjects[i].GetType();
                Type colliderType = collider.GetType();
                if (type.Name == "Floor") //do this first, so we don't even bother to check for collisionwith a floor
                    continue;
                if (levelObjects[i].collisionRect.Intersects(checkCollisionRect))
                {
                    #region player collision
                    if (colliderType.Name == "Player") //check for player collisions
                    {
                        if (type.Name == "Door")
                        {
                            if (!Visible)
                            {
                                Visible = true;
                                return true;
                            }
                            else
                            {
                                if (player.Keys > 0)
                                {
                                    player.Keys = -1;
                                    levelObjects[i].OnCollision(player);
                                    levelObjects.RemoveAt(i);
                                    --i;
                                    return false;
                                }
                                return true; // this is in case it is visible and player has no keys it stillr eturns true
                            }

                        }//end door collision
                        //need to leave this wall colllsion basically for last since some things inherit wall, we need to check them first
                        if (type.Name == "Wall" ||  type.BaseType.Name == "Wall") // this should block the player from moving ontoa ny solid type object, these objects shoudl inherit from wall for this to work
                        {
                            levelObjects[i].OnCollision(player);
                            return true;
                        }

                        if (type.Name == "TeleportTrap")
                        {
                            levelObjects[i].OnCollision(player);
                            levelObjects.RemoveAt(i);
                            --i;
                            return false;
                        }

                       

                        levelObjects[i].OnCollision(player);
                        levelObjects.RemoveAt(i);
                        --i;
                    }//end player collision section 
                    #endregion 
                    #region Mob Collision 
                    if (colliderType.BaseType.Name == "BaseMob") //if not player must be a mob or something else
                    {
                        if (type.BaseType.Name == "BaseMob")
                            return true;
                        if (type.Name == "Wall" || type.BaseType.Name == "Wall")
                        {
                            Wall wall = (Wall)levelObjects[i];
                            if (wall.Breakable)
                            {
                                Sprite mob = (Sprite)collider;
                                levelObjects.Remove(wall);
                                levelObjects.Remove(mob);
                                return true;
                            }
                            return true;
                        }
                        if (type.Name == "Lava")
                        {
                            Lava lava = (Lava)levelObjects[i];
                            Sprite mob = (Sprite)collider;
                            levelObjects.Remove(lava);
                            levelObjects.Remove(mob);
                            return true;
                        }

                            levelObjects.RemoveAt(i);
                            --i;
                            return false;
                    } //end mob collision check
                    #endregion   
                    #region Whip Collision
                    if (colliderType.Name == "WhipAnimation")
                    {
                        int strength = playerWhip.Strength;

                        if (type.BaseType.Name == "BaseMob")
                        {
                            BaseMob mob = (BaseMob)levelObjects[i];
                            if (playerWhip.collisionRect.Intersects(mob.collisionRect))
                            {
                                levelObjects.RemoveAt(i);
                                --i;
                            }
                        }

                        if (type.Name == "Wall" || type.BaseType.Name == "Wall")
                        {

                            int chanceToBreakWall = (rand.Next(100) / strength);
                            Wall wall = (Wall)levelObjects[i];
                            if (wall.Breakable && chanceToBreakWall <= 25)
                            {
                                levelObjects.RemoveAt(i);
                                --i;
                            }
                        }
                        if (type.Name == "TeleportTrap")
                        {
                            levelObjects.RemoveAt(i);
                            --i;
                        }
                    }
                    #endregion
                } // end check for items that do collide
            } // end for loop to check each item. 
            return false; 
        }

        public Vector2 CheckPlayerCollision(Vector2 newPOS)
        {
            //--------------Check for player collision with thing-------------------------------------//
            Rectangle playerCollisionRect = new Rectangle((int)newPOS.X, (int)newPOS.Y, spriteSize.X, spriteSize.Y);

            bool collided = false;
            //------------------------------LevelObjects Collisions-----------------------------------------------------------//

            for (int i = 0; i < levelObjects.Count; i++)
            {
                Type type = levelObjects[i].GetType();

                if (levelObjects[i].collisionRect.Intersects(playerCollisionRect) && type.Name != "Floor")
                {
                    if (type.Name == "Wall" || type.Name == "MobileWall")
                    {
                        levelObjects[i].OnCollision(player);
                        collided = true;
                        break;
                    }

                    if (type.Name == "TeleportTrap")
                    {
                        levelObjects[i].OnCollision(player);
                        levelObjects.RemoveAt(i);
                        --i;
                        return player.GetPosition;
                    }

                    if (type.Name == "Door")
                    {
                        if (!Visible)
                        {
                            Visible = true;
                            collided = true;
                            break;
                        }
                        else
                        {
                            if (player.Keys > 0)
                            {
                                player.Keys = -1;
                                levelObjects[i].OnCollision(player);
                                levelObjects.RemoveAt(i);
                                --i;
                                break;
                            }

                        }
                    }//end door collision
                }
                //last ditch effort to check collision effect
                levelObjects[i].OnCollision(player);
                levelObjects.RemoveAt(i);
                --i;
            }
            if (!collided)
                playerPOS = newPOS;
            return playerPOS;
        }

        private void ClearLevel()
        {
            this.levelMap.Clear();
            this.levelObjects.Clear();
            this.levelMessages.Clear();
            foreach (KeyValuePair<char, List<Wall>> wallList in wallsToTrigger)
            {
                wallList.Value.Clear();
            }
        }

        private void NewLevel(object sender)
        {
            if (sender is Stairs)
            {
                this.level += 1;
                this.ClearLevel();
                this.LoadLevelMap();
            }
        }
        
        private void LoadLevelMap()
        {
            //Load level from txt file and store ina List<String>
            string levelToLoad = string.Format(@"lvl\lvl{0}.txt", level);

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(levelToLoad);
            while ((line = file.ReadLine()) != null)
            {
                levelMap.Add(line);
            }
            file.Close();
             //grab the map width and heigh, 32 is the current size of sprites in x and y
            this.mapHeight = levelMap.Count() * spriteSize.Y;
            this.mapWidth = levelMap[0].Length * spriteSize.X;
            this.ExtractMap();
        }
        
        private void ExtractMap()
        {
             //---------------------------read through the list<string> and create objects---------------//
             int mapX = 0;
             int mapY = 0;
             Vector2 position;
             /* This is used to grab the data out of the level map and
              * to filter in into objects / lists for easier updating later on
              */
             #region for loop
             for (int y = 0; y < levelMap.Count(); y++)
             {
                 for (int x = 0; x < levelMap[y].Length; x++)
                 {
                     mapX = x * spriteSize.X;
                     mapY = y * spriteSize.Y;
                     position.X = mapX;
                     position.Y = mapY;
                     //position = new Vector2(x, y);
                     //---------------------------FLOORS--------------------------------------------------
                     levelObjects.Add(new Floor(floorTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, white));
                     levelLocations[y, x].Add(new Floor(floorTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, white));
                     //-----------------------REGUALR WALLS----------------------------------------------------------//
                     if (levelMap[y][x] == '#') //gray
                         levelLocations[y, x].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true, white));
                     
                     if (levelMap[y][x] == '4') //gray
                     {
                         wallsToTrigger['4'].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                     }
                     
                     if (levelMap[y][x] == '5') //brown
                     {
                         wallsToTrigger['5'].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                     }
                     
                     if (levelMap[y][x] == '6') //brown
                     {
                         wallsToTrigger['6'].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                     }

                     //---------------------INVISIBLE WALLS--------------------------------------//
                     if(levelMap[y][x] == ':') //invisible
                        levelLocations[y, x].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, false, white)); 
                     
                     if (levelMap[y][x] == ';')
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, false, white)); 
                     //---------------------BREAKABLE WALLS--------------------------------------------------//
                     if(levelMap[y][x] == 'X')
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, white));
                     
                     if (levelMap[y][x] == 'Y')
                     {
                        levelLocations[y, x].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                         wallsToTrigger['y'].Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, gray));
                     }
                     
                     if (levelMap[y][x] == '/')
                        levelLocations[y, x].Add(new Wall(treeWallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true, Color.White));
                     //---------------------WATER-------------------------------------------------------------//
                     if(levelMap[y][x] == 'R')
                        levelLocations[y, x].Add(new Wall(waterTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true, white));
                     //----------------------LAVA--------------------------------------------------------------------
                     if (levelMap[y][x] == 'V')
                     {
                         Lava lava = new Lava(lavaTexture, position, spriteSize, 0, Vector2.Zero, 0, 15000);
                         lava.LavaFlowEvent += new Lava.Flow(this.LavaFlow);
                        levelLocations[y, x].Add(lava);
                     }
                     //----------------------BOTTOMLESS PIT---------------------------------------------------------
                     if (levelMap[y][x] == '=')
                     {
                         BottomlessPit pit = new BottomlessPit(pitTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                         pit.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(pit);
                     }
                     //-----------------------GEMS---------------------------------------------------------------//
                     if (levelMap[y][x] == '+')
                        levelLocations[y, x].Add(new Gem(gemTexture, position, spriteSize, 0, Vector2.Zero, 5, 0)); 
                     //-----------set the player position-------------------------------------//
                     if (levelMap[y][x] == 'P')
                         player.SetPosition = new Vector2(x, y);
                         //player.SetPosition = position;
                     //-----------ITEM POS---------------------------------------
                     if (levelMap[y][x] == '*')
                        levelLocations[y, x].Add(new Gold(goldTexture, position, spriteSize, 0, Vector2.Zero, 50, 0)); 

                     if (levelMap[y][x] == 'W')
                        levelLocations[y, x].Add(new Whip(whipTexture, position, spriteSize, 0, Vector2.Zero, 5, 0));

                     if (levelMap[y][x] == 'C')
                        levelLocations[y, x].Add(new Chest(chestTexture, position, spriteSize, 0, Vector2.Zero, 0, 0)); 

                     if(levelMap[y][x] == 'K')
                        levelLocations[y, x].Add(new Key(keyTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));

                     if (levelMap[y][x] == 'T')
                        levelLocations[y, x].Add(new TeleportScroll(teleportTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));

                     if (levelMap[y][x] == 'D')
                         levelLocations[y, x].Add(new Door(doorTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, true));

                     if (levelMap[y][x] == '^') //invisible door
                        levelLocations[y, x].Add(new Door(doorTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, false));

                     if (levelMap[y][x] == 'I')
                        levelLocations[y, x].Add(new PlayerInvisible(invisTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));

                     if (levelMap[y][x] == 'Q')
                     {
                         WhipPowerRing ring = new WhipPowerRing(powerRingTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                         ring.OnCollisionEvent += new Sprite.CollisionEvent(playerWhip.EventHandler);
                        levelLocations[y, x].Add(ring);
                     }

                     if(levelMap[y][x] == 'S')
                     {
                         SlowMobs slow = new SlowMobs(slowMobTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                         slow.OnCollisionEvent += new Sprite.CollisionEvent(BaseMob.EventHandler);
                        levelLocations[y, x].Add(slow);
                     }

                     if (levelMap[y][x] == '?')
                        levelLocations[y, x].Add(new SackOfGems(sackTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                     
                     if (levelMap[y][x] == 'B')
                     {
                         Bomb bomb = new Bomb(bombTexture, position, spriteSize, 0, Vector2.Zero, 500, 0);
                         bomb.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(bomb);
                     }

                     if (levelMap[y][x] == ']')
                     {
                         AddMobs more = new AddMobs(sackTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                         more.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(more);
                     }
                     
                     if (levelMap[y][x] == '%')
                     {
                         RemoveMobs less = new RemoveMobs(removeMobTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                         less.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(less);
                     }

                     if(levelMap[y][x] == 'G')
                     {
                         MobGenerator generator = new MobGenerator(mobGeneratortexture, position, spriteSize, 0, Vector2.Zero, 0, 5000);
                         generator.GenerateMobEvent += new MobGenerator.Generate(this.GenerateMobEventHandler);
                        levelLocations[y, x].Add(generator);
                     }
                     if (levelMap[y][x] == '>')
                     {
                         Statue statue = new Statue(playerTexture, position, spriteSize, 0, Vector2.Zero, 500, 1000);
                         statue.CheckPlayerDistanceEvent += new Statue.StatueEvent(this.EventHandler);
                        levelLocations[y, x].Add(statue);
                     }
                     if (levelMap[y][x] == 'U')
                     {
                         Teleporter teleporter = new Teleporter(teleporterTexture, position, spriteSize, 0, Vector2.Zero, 0, 0,Color.White);
                         teleporter.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(teleporter);
                     }
                     //----------------KROZ letters----------------------------------------------------------------------
                     if (levelMap[y][x] == '<')
                        levelLocations[y, x].Add(new KrozLetter(kTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, 'k'));
                    
                     if (levelMap[y][x] == '[')
                        levelLocations[y, x].Add(new KrozLetter(rTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, 'r'));
                     
                     if (levelMap[y][x] == '|')
                        levelLocations[y, x].Add(new KrozLetter(oTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, 'o'));
                     
                     if (levelMap[y][x] == '"')
                        levelLocations[y, x].Add(new KrozLetter(zTexture, position, spriteSize, 0, Vector2.Zero, 0, 0, 'z'));

                     //----------------FREEZE MOBS-------------------------------------------------------------------------------//
                    if (levelMap[y][x] == 'Z')
                    {
                        FreezeMobs freeze = new FreezeMobs(freezeTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                        freeze.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                       levelLocations[y, x].Add(freeze);
                    }
                     //--------------------STAIRS------------------------------------------------------------------------
                    if (levelMap[y][x] == 'L')
                    {
                        Stairs exit = new Stairs(stairsTexture, position, spriteSize);
                        exit.OnStairs += new Stairs.FiredEvent(this.NewLevel);
                       levelLocations[y, x].Add(exit);
                    }
                     //---------------ANCIENT TABLET------------------------------------------------------------------------//
                    if (levelMap[y][x] == '!')
                       levelLocations[y, x].Add(new AncientTablet(ancientTabletTexture, position, spriteSize, 0, Vector2.Zero, 1000, 0, this.level));

                        //--------------TRIGGERS-------------------------------------------------------------------------
                    if (levelMap[y][x] == '.')
                    {
                        TeleportTrap trap = new TeleportTrap(teleportTrapTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                        trap.SetEmptySpaces = FindEmptySpaces(false);
                       levelLocations[y, x].Add(trap);
                    }

                    if (levelMap[y][x] == 'H')
                    {
                        AddRandomGemsTrigger trigger = new AddRandomGemsTrigger(transparentTexture, position, spriteSize);
                        trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                       levelLocations[y, x].Add(trigger);
                    }

                    if (levelMap[y][x] == 'A')
                    {
                        ActivateMobileWallTrigger wallTrigger = new ActivateMobileWallTrigger(transparentTexture, position, spriteSize);
                        wallTrigger.OnCollisionEvent += new ActivateMobileWallTrigger.ActivateWalls(MobileWall.ActivateEventHandler);
                       levelLocations[y, x].Add(wallTrigger);
                    }

                     if(levelMap[y][x] == 'E')
                     {
                         Earthquake quake = new Earthquake(transparentTexture, position, spriteSize);
                         quake.StartEarthquake += new Earthquake.QuakeEvent(this.EventHandler);
                        levelLocations[y, x].Add(quake);
                     }
                     
                     if(levelMap[y][x] == 'N')
                     {
                         HideWalls hide = new HideWalls(transparentTexture, position, spriteSize);
                         hide.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(hide);
                     }

                     //---------------HIDDEN WALL 9 TRIGGER----------------------------------------//
                     if (levelMap[y][x] == '$')
                     {
                         AddWallsTrigger trigger = new AddWallsTrigger(transparentTexture, position, spriteSize, '9');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }
                     if (levelMap[y][x] == '9')
                         wallsToTrigger['9'].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true, white));
                     //---------------HIDDEN WALL '8' TRIGGER -------------------------------------//
                     if (levelMap[y][x] == ',')
                     {
                         AddWallsTrigger trigger = new AddWallsTrigger(transparentTexture, position, spriteSize, '8');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }

                    if (levelMap[y][x] == '8')
                        wallsToTrigger['8'].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true, white));
                     //-------------HIDDEN WALL '7' TRIGGER ----------------------------------------------------------//
                    if (levelMap[y][x] == '`')
                    {
                        AddWallsTrigger trigger = new AddWallsTrigger(transparentTexture, position, spriteSize, '7');
                        trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                       levelLocations[y, x].Add(trigger);
                    }

                     if (levelMap[y][x] == '7')
                         wallsToTrigger['7'].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true, white));
                     //--------------REMOVE WALLS TRIGGERs-------------------------------------
                     if (levelMap[y][x] == '~')
                     {
                         RemoveWallsTrigger trigger = new RemoveWallsTrigger(transparentTexture, position, spriteSize, 'y');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }

                     if (levelMap[y][x] == '@')
                     {
                         RemoveWallsTrigger trigger = new RemoveWallsTrigger(transparentTexture, position, spriteSize, '6');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }
                     
                     if (levelMap[y][x] == ')')
                     {
                         RemoveWallsTrigger trigger = new RemoveWallsTrigger(transparentTexture, position, spriteSize, '5');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }

                     if (levelMap[y][x] == '(')
                     {
                         RemoveWallsTrigger trigger = new RemoveWallsTrigger(transparentTexture, position, spriteSize, '4');
                         trigger.OnCollisionEvent += new Sprite.CollisionEvent(this.EventHandler);
                        levelLocations[y, x].Add(trigger);
                     }
                     //----------------------MOBS--------------------------------------------//
                    if (levelMap[y][x] == 'a')//a
                        levelLocations[y, x].Add(new Gnome(gnomeTexture, position = new Vector2(x, y), spriteSize, 0, new Vector2(1, 1), 5, 1000));
                   
                     if (levelMap[y][x] == 'b') //b
                         levelLocations[y, x].Add(new ElfMummy(elfMummyTexture, position = new Vector2(x, y), spriteSize, 0, new Vector2(1, 1), 10, 750));

                     if (levelMap[y][x] == 'c') //c
                         levelLocations[y, x].Add(new OgreLord(ogreLordTexture, position = new Vector2(x, y), spriteSize, 0, new Vector2(1, 1), 15, 500));

                     if (levelMap[y][x] == 'd') //d
                         levelLocations[y, x].Add(new UmberHulk(umberHulkTexture, position = new Vector2(x, y), spriteSize, 0, new Vector2(1, 1), 20, 250));

                     //bubble monster can be e
                     if (levelMap[y][x] == 'M')
                         levelLocations[y, x].Add(new MobileWall(breakableTexture, position = new Vector2(x, y), spriteSize, 0, new Vector2(1, 1), 10, 1500));
                 }//end inner for loop
             }//end the for loops that look through the entire map and create the contents
             #endregion
        }//END EXTRACT MAP


        void EventHandler(object sender)
        {
            if (sender is BottomlessPit)
                this.PitFall();
            
            if (sender is AddMobs)
            {
                List<Vector2> emptySpaces = FindEmptySpaces(false);
                for (int i = 0; i < 10; i++)
                {
                    int pos = rand.Next(emptySpaces.Count);
                    levelObjects.Add(new ElfMummy(elfMummyTexture, emptySpaces[pos], spriteSize, 0, new Vector2(1, 1), 10, 750));
                    emptySpaces.RemoveAt(pos);

                }
            }

            if (sender is RemoveMobs)
            {
                for (int i = 0; i < 10; i++)
                {
                    int toRemove = rand.Next(levelObjects.Count);
                    levelObjects.RemoveAt(toRemove);
                }
            }

            if (sender is Earthquake)
            {
                List<Vector2> emptySpaces = FindEmptySpaces(false);
                int newWalls = rand.Next(10, 20);
                for (int i = 0; i < newWalls; i++)
                {
                    int pos = rand.Next(emptySpaces.Count);
                    levelObjects.Add(new Wall(breakableTexture, emptySpaces[pos], spriteSize, 0, Vector2.Zero, 0, 0, true, true, Color.Chocolate));
                    emptySpaces.RemoveAt(pos);
                }
            }

            if (sender is HideWalls)
            {
                int hideWalls = rand.Next(20, 40);
                for (int i = 0; i < hideWalls; i++)
                {
                    int pos = rand.Next(levelObjects.Count);
                    if (levelObjects[pos].GetType().Name == "Wall")
                    {
                        levelObjects[pos].Visible = false;
                        continue;
                    }
                    --i;
                }
            }

            if (sender is Bomb)
            {
                Bomb bomb = (Bomb)sender;
                Point bombCenter = bomb.collisionRect.Center;
                Vector2 radius = new Vector2(spriteSize.X * 5, spriteSize.Y * 5);
                for (int i = 0; i < levelObjects.Count; i++)
                {
                    if (levelObjects[i].GetType().Name == "Wall")
                    {
                        Wall wall = (Wall)levelObjects[i];
                        Point wallCenter = wall.collisionRect.Center;
                        if (inRange(bombCenter, wallCenter, radius) && wall.Breakable)
                        {
                            levelObjects.RemoveAt(i);
                            --i;
                        }
                    }
                }
            } //end bomb collision

            if (sender is Stairs)
            {
                this.level += 1;
                this.ClearLevel();
                this.LoadLevelMap();
            }

            if (sender is AddRandomGemsTrigger)
            {
                int randomNumOfGems = rand.Next(10, 20);
                List<Vector2> emptySpaces = FindEmptySpaces(false);
                for (int i = 0; i < randomNumOfGems; i++)
                {
                    int randomNum = rand.Next(0, emptySpaces.Count);
                    levelObjects.Add(new Gem(gemTexture, emptySpaces[randomNum], spriteSize, 0, Vector2.Zero, 5, 0));
                    emptySpaces.RemoveAt(i);
                }
            }

            if (sender is AddWallsTrigger)
            {
                AddWallsTrigger trigger = (AddWallsTrigger)sender;
                levelObjects.AddRange(wallsToTrigger[trigger.Trigger]); 
            }
            if (sender is RemoveWallsTrigger)
            {
                RemoveWallsTrigger trigger = (RemoveWallsTrigger)sender;
                char key = trigger.Trigger;
                for (int i = 0; i < wallsToTrigger[key].Count; i++)
                {
                    Wall wallToRemove = (Wall)wallsToTrigger[key][i];
                    for (int j = 0; j < levelObjects.Count; j++)
                    {
                        if (levelObjects[j].GetType().Name == "Wall")
                        {
                            Wall wallToCheck = (Wall)levelObjects[j];
                            if (wallToRemove.collisionRect.Intersects(wallToCheck.collisionRect))
                                levelObjects.Remove(wallToCheck);
                        }
                    }
                    
                }
            }//end remove walls trigger

            if (sender is Statue)
            {
                Statue statue = (Statue)sender;
                if (statue.CheckDistanceToPlayer(player) <= 2)
                {
                    levelMessages.Add("An evil force is draining your life!");
                    player.Health = -2;
                }
            }

            if (sender is Teleporter)
            {

            }
        } 
        // end CollisionHandler

         void Teleport()
         {
             List<Vector2> teleportSpace = new List<Vector2>();
             teleportSpace = FindEmptySpaces(false);
             int randomNum = rand.Next(0, teleportSpace.Count);
             player.SetPosition = teleportSpace[randomNum];

         }

         void LavaFlow(Lava lava)
         {
                 if (lava.Flowing)
                 {
                     int size = spriteSize.X;
                     Vector2 lavaUp = new Vector2((int)lava.GetPosition.X - size, (int)lava.GetPosition.Y);
                     Vector2 lavaDown = new Vector2((int)lava.GetPosition.X + size, (int)lava.GetPosition.Y);
                     Vector2 lavaLeft = new Vector2((int)lava.GetPosition.X, (int)lava.GetPosition.Y - size);
                     Vector2 lavaRight = new Vector2((int)lava.GetPosition.X, (int)lava.GetPosition.Y + size);

                     if (CheckCollisions(lavaUp, true) && CheckCollisions(lavaDown, true) &&
                         CheckCollisions(lavaLeft, true) && CheckCollisions(lavaRight, true))
                     {
                         lava.Flowing = false;
                     }
                     else
                     {
                         if (!CheckCollisions(lavaUp, true))
                         {
                             Lava newLava = new Lava(lavaTexture, lavaUp, spriteSize, 0, Vector2.Zero, 0, 15000);
                             newLava.LavaFlowEvent += new Lava.Flow(this.LavaFlow);
                             levelObjects.Add(newLava);
                         }
                         if (!CheckCollisions(lavaDown, true))
                         {
                             Lava newLava = new Lava(lavaTexture, lavaDown, spriteSize, 0, Vector2.Zero, 0, 15000);
                             newLava.LavaFlowEvent += new Lava.Flow(this.LavaFlow);
                             levelObjects.Add(newLava);
                         }
                         if (!CheckCollisions(lavaLeft, true))
                         {
                             Lava newLava = new Lava(lavaTexture, lavaLeft, spriteSize, 0, Vector2.Zero, 0, 15000);
                             newLava.LavaFlowEvent += new Lava.Flow(this.LavaFlow);
                             levelObjects.Add(newLava);
                         }
                         if (!CheckCollisions(lavaRight, true))
                         {
                             Lava newLava = new Lava(lavaTexture, lavaRight, spriteSize, 0, Vector2.Zero, 0,15000);
                             newLava.LavaFlowEvent += new Lava.Flow(this.LavaFlow);
                             levelObjects.Add(newLava);
                         }
                     }


                 }
             }
            
         void GenerateMobEventHandler()
         {
             //use this when a mob generator fires off an event to create a new mob some wher ein a blank space
             List<Vector2> emptySpaces = FindEmptySpaces(false);
             int pos = rand.Next(emptySpaces.Count);
             levelObjects.Add(new ElfMummy(elfMummyTexture, emptySpaces[pos], spriteSize, 0, new Vector2(1, 1), 10, 750));

         }
       
         bool inRange(Point epicenter, Point toCheck, Vector2 radius) //to check if something is withina certain distance
         {
              double square_dist = Math.Pow((epicenter.X - toCheck.X), 2) + Math.Pow((epicenter.Y - toCheck.Y), 2);
              return square_dist <= Math.Pow(radius.X, 2);
         }

        //use these to have the main program kroz.exe switch game states to the pitfall screen.
         public delegate void PitEvent();
         public event PitEvent PitFall;

    }
}