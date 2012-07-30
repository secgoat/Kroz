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


namespace Kroz
{
    public class LevelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //test variables for event handling
        FreezeMobs freezeSpell;

        #region variables
        public static Point spriteSize; //in pixels each direction
        SpriteBatch spriteBatch;
        SpriteFont baseFont;

        //--------------Input from keyboard----------------------//
        KeyboardState keyboardState;
        KeyboardState prevKeyboardState;
        //--------------viewport ------------------------------//
        Viewport mapViewport;

        //--------------player--------------------------------//
        Player player;
        int playerX;
        Vector2 playerPOS = Vector2.Zero;
        Vector2 playerOldPOS = Vector2.Zero;
        Vector2 playerNewPOS = Vector2.Zero;
        WhipAnimation playerWhip;
        //------------------textures for walls etc-------------//
        #region textures
        Texture2D ancientTabletTexture;
        Texture2D breakableTexture;
        Texture2D chestTexture;
        Texture2D doorTexture;
        Texture2D floorTexture;
        Texture2D freezeTexture;
        Texture2D gemTexture;
        Texture2D goldTexture;
        Texture2D keyTexture;
        Texture2D playerTexture;
        Texture2D stairsTexture;
        Texture2D teleportTexture;
        Texture2D wallTexture;
        Texture2D waterTexture;
        Texture2D whipAnimation;
        Texture2D whipTexture;
        //MOb textures
        Texture2D gnomeTexture;
        #endregion

        //---------keep track of level number and the current map--------------//
        private int level;
        private List<string> levelMap;
        private int mapWidth; 
        private int mapHeight;
        //---------map level vlists to keep track of items walls etc. -----------//
        private List<BaseMob>                       levelMobs = new List<BaseMob>();
        private List<BaseItem>                      levelItems = new List<BaseItem>(); //everything should inherit from item, walls, gems etc. This should then hold pretty much everything.
        List<Wall>                                  levelWalls = new List<Wall>();
        private List<Door>                          levelDoors = new List<Door>();
        private List<BaseTrigger>                   levelTriggers = new List<BaseTrigger>();
        private Dictionary<int, AddWallsTrigger>    levelHiddenWallsTriggers = new Dictionary<int, AddWallsTrigger>();
        private Dictionary<int, List<Wall> >        levelTempHiddenWalls = new Dictionary<int, List<Wall> >(); // use this to keep track of hidden walls and pass to the appropriate trigger based on key
        List<Stairs>                                levelStairs = new List<Stairs>();

        //--------------Random number genrartor for good measure-------------------------------------------//
        Random rand = new Random();

        //--------------VARIABLES TO KEEP TRACK OF MESSAGES-----------------------------------------------//
        List<String> levelMessages = new List<String>();

        #endregion

        public int Level { get { return level; } set { level = value; } }
        public List<Vector2> AddGems // get a list of 3 ints: number of new gems , x pos and y pos
        { 
            set 
            {
                for (int i = 1; i <= value[0].X; i++)
                {
                    levelItems.Add(new Gem(gemTexture, new Vector2(value[i].X, value[i].Y), spriteSize, 0, Vector2.Zero, 5, 0));
                }
            } 
        }
        public List<Wall> AddWalls{ set { levelWalls.AddRange(value); } }


        public LevelManager(Game game) : base(game)
        {
            level = 1;
            levelMap = new List<string>();
            spriteSize = new Point(16, 16); //set the sprite size here
            this.LoadLevelMap();
            
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            baseFont = Game.Content.Load<SpriteFont>("BaseFont");
            //-------------LOAD TEXTURES------------------------------//
            ancientTabletTexture = Game.Content.Load<Texture2D>(@"Images\tablet");
            breakableTexture = Game.Content.Load <Texture2D>(@"Images\breakable");
            chestTexture =  Game.Content.Load<Texture2D>(@"Images\chest");
            doorTexture =   Game.Content.Load<Texture2D>(@"Images\door");
            floorTexture =  Game.Content.Load<Texture2D>(@"Images\floor");
            freezeTexture = Game.Content.Load<Texture2D>(@"Images\freeze_monester");
            gemTexture =    Game.Content.Load<Texture2D>(@"Images\gem");
            gnomeTexture = Game.Content.Load<Texture2D>(@"Images\gnome");
            goldTexture =   Game.Content.Load<Texture2D>(@"Images\gold");
            keyTexture = Game.Content.Load<Texture2D>(@"Images\key");
            stairsTexture = Game.Content.Load<Texture2D>(@"Images\stairs");
            teleportTexture = Game.Content.Load<Texture2D>(@"Images\teleport");
            wallTexture =   Game.Content.Load<Texture2D>(@"Images\wall");
            waterTexture =  Game.Content.Load<Texture2D>(@"Images\water");
            whipAnimation = Game.Content.Load<Texture2D>(@"Images\whipAnimation16"); //drop the 16 when you got to 32x32 tiles
            whipTexture =   Game.Content.Load<Texture2D>(@"Images\whip");
            
            player = new Player(Game.Content.Load<Texture2D>(@"Images\player"), new Vector2(32,32),
                spriteSize, 2, Vector2.Zero, 0, 350);
            playerWhip = new WhipAnimation(whipAnimation, Vector2.Zero, spriteSize, 0, Vector2.Zero, 0, 350);
            this.ExtractMap();
            base.LoadContent();
        }
        
        public override void Update(GameTime gameTime)
        {
            /*Use this to update the players movement and check for collision detecion
             *we keep it at this level in the LevelManager so that we can keep track off all sprites items etc 
             *and player / item doesnt have to keep track of world etc.
             */
            Vector2 inputDirection = Vector2.Zero;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left)) //move left
                inputDirection.X -= spriteSize.X;
            if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right)) //move right
                inputDirection.X += spriteSize.X; 
            if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up)) //move up
                inputDirection.Y -= spriteSize.Y;
            if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down)) //move down
                inputDirection.Y += spriteSize.Y;

            prevKeyboardState = keyboardState; //keep this here as I only care about prevKeyboard state on movement i think
            
            if (keyboardState.IsKeyDown(Keys.Space)) //whip
                playerWhip.Whip = true;
            if (keyboardState.IsKeyDown(Keys.T)) // teleport
                    Teleport();
            if(keyboardState.IsKeyDown(Keys.Escape))//exit
                Game.Exit();

            playerNewPOS = player.GetPosition + inputDirection; //make the next move based onb player position and what key is pressed
            player.SetPosition = CheckPlayerCollision(playerNewPOS); //check to see if player runs into anything, and set position accordingly
            player.Update(gameTime, Game.Window.ClientBounds);
            playerWhip.Update(gameTime, Game.Window.ClientBounds, player);
            CheckWhipCollision();
            //update mobs
            for(int i = 0; i < levelMobs.Count; i++)
            //foreach (BaseMob mob in levelMobs)
            {
                Vector2 dir = Vector2.Zero;
                Vector2 newMobPos = Vector2.Zero;
                Vector2 mobPos = Vector2.Zero;
                BaseMob mob = levelMobs[i];
                dir = mob.DirToPlayer(player);
                newMobPos = mob.GetPosition + dir;

                //mob.SetPosition = CheckMobCollision(newMobPos, mob);
                mob.Update(gameTime, Game.Window.ClientBounds, newMobPos, this);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int mapX = 0;
            int mapY = 0;
            int testX = 0;
            spriteBatch.Begin();

            for (int y = 0; y < levelMap.Count(); y++)
            {
                //for (int x = 0; x < levelMap[y].Length; x++)
                testX = (playerX - 8 < 0) ? 0 : playerX - 8; 
                    for (int x = testX; x < levelMap[y].Length; x++)
                {
                    mapX = x * spriteSize.X;
                    mapY = y * spriteSize.Y;
                    //-----------Draw the floor texture on each space-------------------------------//
                    spriteBatch.Draw(floorTexture, new Rectangle(mapX, mapY, spriteSize.X, spriteSize.Y), Color.White);
                    //--------------# is a wall space, draw that now --------------------------------//
                }

            }
            foreach (Stairs stair in levelStairs)
                spriteBatch.Draw(stairsTexture, stair.collisionRect, Color.White);
            foreach (Wall wall in levelWalls){ 
                if (wall.Visible)
                    wall.Draw(gameTime, spriteBatch);
            }
            foreach (BaseItem item in levelItems) { item.Draw(gameTime, spriteBatch); }
            foreach (Door door in levelDoors) { door.Draw(gameTime, spriteBatch);  }
            foreach (BaseMob mob in levelMobs) { mob.Draw(gameTime, spriteBatch); }
            player.Draw(gameTime, spriteBatch);
            //spriteBatch.DrawString(baseFont, "position newPOS oldPOS " + player.GetPosition + playerNewPOS + playerOldPOS, new Vector2(10, 410), Color.White);
            spriteBatch.DrawString(baseFont, "Gems:      " + player.Health, new Vector2(10, 400), Color.White);
            spriteBatch.DrawString(baseFont, "Whips:     " + player.Whips, new Vector2(10, 420), Color.White);
            spriteBatch.DrawString(baseFont, "Keys:      " + player.Keys, new Vector2(10, 440), Color.White);
            spriteBatch.DrawString(baseFont, "Teleports: " + player.Teleports, new Vector2(10, 460), Color.White);
            spriteBatch.DrawString(baseFont, "Score:     " + player.Score, new Vector2(10, 480), Color.White);
            levelMessages.Reverse();
            for (int i = 0; i < levelMessages.Count; i++)
            {
                spriteBatch.DrawString(baseFont, levelMessages[i], new Vector2(200, (i * 20) + 400), Color.White);
            }
            levelMessages.Reverse();
            playerWhip.Draw(gameTime, spriteBatch);  
            spriteBatch.End();
            base.Draw(gameTime);
        }
//----------------------My functions not XNA ones------------------------------------------------------------------------------//
        //--------------Check for player collision with thing-------------------------------------//
        public Vector2 CheckPlayerCollision(Vector2 newPOS)
        {
            Rectangle playerCollisionRect = new Rectangle((int)newPOS.X, (int)newPOS.Y, spriteSize.X, spriteSize.Y);

            bool collided = false;
            //------------------------------WALL COLLISION-----------------------------------------------------------//
            for (int i = 0; i < levelWalls.Count; ++i)
            {
                Wall wall = levelWalls[i];
                //check for wall collisions
                if (wall.collisionRect.Intersects(playerCollisionRect))
                {
                    wall.OnCollision(player);
                    collided = true;
                    break;
                }
            }
            /////----------------------ITEM COLLISION-------------------------------------------------////////////
            for (int i = 0; i < levelItems.Count; i++)
            {
                BaseItem item = levelItems[i];
                if (item.collisionRect.Intersects(playerCollisionRect))
                {
                    item.OnCollision(player);
                    levelMessages.Add(item.Description);
                    levelItems.RemoveAt(i);
                    --i;
                }
            }
            //---------------_DOOR COLLISIONS-----------------------------------------------------------------------//
            for (int i = 0; i < levelDoors.Count; i++)
            {
                Door door = levelDoors[i];
                if(door.collisionRect.Intersects(playerCollisionRect))
                {
                    bool unlocked = false;
                    unlocked = door.Unlock(player);
                    if(unlocked)
                    {
                        levelDoors.RemoveAt(i);
                        --i;
                        break;
                    }
                    else
                    {
                        collided = true;
                        break;
                    }
                }
            }
            //----------------Check for collision with triggers---------------------------------------------------//
            for (int i = 0; i < levelTriggers.Count; i++)
            {
                BaseTrigger trigger = levelTriggers[i];
                if (trigger.collisionRect.Intersects(playerCollisionRect))
                {

                    trigger.SetEmptySpaces = FindEmptySpaces();
                    string triggerType = trigger.ToString();
                    if (triggerType == "Kroz.Triggers.AddRandomGemsTrigger")
                    {
                        List<Vector2> positions = new List<Vector2>();
                        trigger.OnCollision(this);
                    }
                    trigger.OnCollision(player);
                    levelTriggers.RemoveAt(i);
                    --i;
                    return playerPOS = player.GetPosition; // do this other wise we get to the end and set player POS to 
                    //new pos instead of the teleport trigger posiiton
                }
            }
            for (int j = 0; j < levelHiddenWallsTriggers.Count; j++)
            {
                AddWallsTrigger trigger = levelHiddenWallsTriggers.ElementAt(j).Value;
                if (trigger.collisionRect.Intersects(player.collisionRect))
                {
                    trigger.OnCollision(this);
                    levelHiddenWallsTriggers.Remove(j);
                    //--j;
                    return playerPOS = newPOS;
                }
            }
            for(int i = 0; i < levelMobs.Count; i++)
            {
                BaseMob mob = levelMobs[i];
                if(mob.collisionRect.Intersects(player.collisionRect))
                {
                    mob.OnCollision(player);
                    levelMobs.RemoveAt(i);
                    --i;
                }
            }

            /*-------this is at the end to make sure player hasnt collided 
             * with anythign solid, if so move him ot the new space----*/
            if (!collided)
                playerPOS = newPOS;

            return playerPOS;
        }

        public void CheckWhipCollision()
        {
            for (int i = 0; i < levelMobs.Count; i++)
            {
                BaseMob mob = levelMobs[i];
                if(playerWhip.collisionRect.Intersects(mob.collisionRect))
                {
                    levelMobs.RemoveAt(i);
                    --i;
                }
            }
            for (int i = 0; i < levelWalls.Count; i++)
            {
                Wall wall = levelWalls[i];
                if (wall.Breakable && wall.collisionRect.Intersects(playerWhip.collisionRect))
                {
                    levelWalls.RemoveAt(i);
                    --i;
                }
                
            }

        }

        public Vector2 CheckMobCollision(Vector2 newPOS, BaseMob mob)
        {
            Vector2 mobPOS = Vector2.Zero;
            Rectangle collisionRect = new Rectangle((int)newPOS.X, (int)newPOS.Y, spriteSize.X, spriteSize.Y);
            for ( int i = 0; i < levelWalls.Count; i++)
            {
                Wall wall = levelWalls[i];
                //check for wall collisions
                if (wall.collisionRect.Intersects(collisionRect))
                {
                    if (wall.Breakable)
                    {
                        levelWalls.RemoveAt(i);
                        --i;
                        levelMobs.Remove(mob);
                        //return Vector2.Zero;
                    }
                    return mob.GetPosition;
                }
            }
            return mobPOS + newPOS;
        }

        private void LoadLevelMap()
        {
            //Load level from txt file and store ina List<String>
            string levelToLoad = string.Format(@"lvl\lvl{0}.txt", level);

            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(levelToLoad);
            //System.IO.StreamReader file = new System.IO.StreamReader(@"lvl\lvl1.txt");
            while ((line = file.ReadLine()) != null)
            {
                levelMap.Add(line);
            }
            file.Close();
             //grab the map width and heigh, 32 is the current size of sprites in x and y
            this.mapHeight = levelMap.Count() * spriteSize.Y;
            this.mapWidth = levelMap[0].Length * spriteSize.X;

        }
        
         private void ExtractMap()
         {
             /* think about grabbib a list of each character and its POS and then iterate through the lists to create the items
              * so that we can do things like mobs first then spells or whatever so we can add the event listserns when we loop
              * through the first time instead of having to loop through a secodn time 
              */
             //---------------------------read through the list<string> and create objects---------------//
             int mapX = 0;
             int mapY = 0;
             Vector2 position;
             /* This is used to grab the data out of the level map and
              * to filter in into objects / lists for easier updating later on
              */
             for (int y = 0; y < levelMap.Count(); y++)
             {
                 for (int x = 0; x < levelMap[y].Length; x++)
                 {
                     mapX = x * spriteSize.X;
                     mapY = y * spriteSize.Y;
                     position.X = mapX;
                     position.Y = mapY;
                     //-----------------------REGUALR WALLS----------------------------------------------------------//
                     if (levelMap[y][x] == '#' || levelMap[y][x] == '6') //just covering the bases both are wall types
                         levelWalls.Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                     
                     //---------------------INVISIBLE WALLS-----------------------------------------------------------------//
                     if(levelMap[y][x] == ':')
                         levelWalls.Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, false));
                     
                     //---------------------BREAKABLE WALLS--------------------------------------------------------//
                     if(levelMap[y][x] == 'X')
                         levelWalls.Add(new Wall(breakableTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, true, true));
                     
                     //---------------------WATER-------------------------------------------------------------//
                     if(levelMap[y][x] == 'R')
                         levelWalls.Add(new Wall(waterTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                     
                     //-----------------------GEMS---------------------------------------------------------------//
                     if(levelMap[y][x] == '+') 
                         levelItems.Add(new Gem(gemTexture, position, spriteSize, 0, Vector2.Zero, 5, 0));
                     
                     //-----------set the player position---------------------------------------------------------------------//
                     if (levelMap[y][x] == 'P')
                         player.SetPosition = position;
                     if(levelMap[y][x] == '*')
                         levelItems.Add(new Gold(goldTexture, position, spriteSize, 0, Vector2.Zero, 50, 0));
                     if(levelMap[y][x] == 'W')
                         levelItems.Add(new Whip(whipTexture, position, spriteSize, 0, Vector2.Zero, 5, 0));
                     if(levelMap[y][x] == 'C')
                         levelItems.Add(new Chest(chestTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                     if(levelMap[y][x] == 'K')
                         levelItems.Add(new Key(keyTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                    if(levelMap[y][x] == 'T')
                        levelItems.Add(new TeleportScroll(teleportTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                    if(levelMap[y][x] == 'D')
                        levelDoors.Add(new Door(doorTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                     //----------------FREEZE MOBS-------------------------------------------------------------------------------//
                    if (levelMap[y][x] == 'Z')
                        //freezeSpell = new FreezeMobs(freezeTexture, position, spriteSize, 0, Vector2.Zero, 0, 0);
                        levelItems.Add(new FreezeMobs(freezeTexture, position, spriteSize, 0, Vector2.Zero, 0, 0));
                     //STAIRS
                    if (levelMap[y][x] == 'L')
                        levelStairs.Add(new Stairs(position, spriteSize));
                     //---------------ANCIENT TABLET------------------------------------------------------------------------//
                     if(levelMap[y][x] == '!')
                         levelItems.Add(new AncientTablet(ancientTabletTexture, position, spriteSize, 0, Vector2.Zero, 1000, 0, this.level));
                     //--------------TELEPORT TRIGGER-------------------------------------------------------------------------
                    if (levelMap[y][x] == '.')
                        levelTriggers.Add(new TeleportTrigger(position, spriteSize));
                     
                     //----------------GEM TRIGGER------------------------------------------------------------------------//
                     if(levelMap[y][x] == 'H')
                         levelTriggers.Add(new AddRandomGemsTrigger(position, spriteSize)); 
                     
                     //---------------HIDDEN WALL '8' TRIGGER --------------------------------------------------------------//

                     if (levelMap[y][x] == ',')
                         levelHiddenWallsTriggers.Add(0, new AddWallsTrigger(position, spriteSize)); //using numbers as the keys so that i can loop through on a for loop
                     if (levelMap[y][x] == '8')
                     {
                         if (levelHiddenWallsTriggers.ContainsKey(0))
                             levelHiddenWallsTriggers[0].AddWall = new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true);
                         else
                         {
                             if (!levelTempHiddenWalls.ContainsKey(0))
                             {
                                 levelTempHiddenWalls.Add(0, new List<Wall>());
                                 levelTempHiddenWalls[0].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                             }
                             else
                                 levelTempHiddenWalls[0].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                         }
                     }//end creation for tile '8'
                     //-------------HIDDEN WALL '7' TRIGGER --------------------------------------------------------------------------//
                     if(levelMap[y][x] == '`')
                         levelHiddenWallsTriggers.Add(1, new AddWallsTrigger(position, spriteSize)); //using numbers as the keys so that i can loop through on a for loop
                     if (levelMap[y][x] == '7')
                     {
                         if (levelHiddenWallsTriggers.ContainsKey(1))
                             levelHiddenWallsTriggers[1].AddWall = new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true);
                         else
                         {
                             if (!levelTempHiddenWalls.ContainsKey(1))
                             {
                                 levelTempHiddenWalls.Add(1, new List<Wall>());
                                 levelTempHiddenWalls[1].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                             }
                             else
                                 levelTempHiddenWalls[1].Add(new Wall(wallTexture, position, spriteSize, 2, Vector2.Zero, 0, 0, false, true));
                         }
                     }//end creation for tile '7'
 
                     //----------------------MOBS----------------------------------------------------------------------------//
                     if (levelMap[y][x] == '1')
                         levelMobs.Add(new Gnome(gnomeTexture, position, spriteSize, 0, new Vector2(1, 1), 5, 1000));
                 }//end inner for loop
             }//end the for loops that look through the entire map and create the contents

             //------------------------LOOP THOUGH ALL KEYS AND THEIR LISTS AND ADD THEM TO THE APPROPRIATE TRIGGERS------------------------//
              if (levelTempHiddenWalls.Count > 0)
                 for (int i = 0; i < levelTempHiddenWalls.Count; i++)
                 {
                     for(int j = 0; j < levelTempHiddenWalls[i].Count; j++)
                        levelHiddenWallsTriggers[i].AddWall = levelTempHiddenWalls[i][j];
                 }
              
             //-------------------ADD LISTENER TO FREEZE SPELL----------------------------------------------------------------//
             /*if (freezeSpell != null)
              {
                  for (int i = 0; i < levelMobs.Count; i++)
                  {
                      BaseMob mob = levelMobs[i];
                      freezeSpell.OnCollisionEvent += new FreezeMobs.FreezeEvent(mob.EventHandler);
                  }
                  levelItems.Add(freezeSpell);
              } */
              for (int i = 0; i < levelItems.Count; i++)
              {
                  BaseItem item = levelItems[i];
                  if (item.Type == "freeze mobs")
                  {
                      for (int j = 0; j < levelMobs.Count; j++)
                      {
                          BaseMob mob = levelMobs[j];
                          //FreezeMobs freeze;
                          //freeze = item as FreezeMobs;
                          item.OnCollisionEvent += new BaseItem.ItemEvent(mob.EventHandler);
                      }
                         
                  }
             }
         }//END EXTRACT MAP

         public List<Vector2> FindEmptySpaces()
         {
             //use this to find empty spaces on the map for things like player teleport, teleport traps, and the gem triggers etc.
             List<Vector2> emptySpaces = new List<Vector2>();
             Vector2 position;
             int mapX = 0;
             int mapY = 0;
                 for (int y = 1; y < levelMap.Count() - 1; y++)
                 {
                     for (int x = 1; x < levelMap[y].Length - 1; x++)
                     {
                         mapX = x * spriteSize.X;
                         mapY = y * spriteSize.Y;
                         position = new Vector2(mapX, mapY);

                         if (!CheckCollisions(position))
                             emptySpaces.Add(position);

                     }
                 }

             return emptySpaces;
         }

         public bool CheckCollisions(Vector2 POS)
         {
             //----------------------used in conjunction with FindEmptySpace to facilitate finding empty spaces
             Rectangle collisionRect = new Rectangle((int)POS.X, (int)POS.Y, spriteSize.X, spriteSize.Y);
             for (int i = 0; i < levelWalls.Count; ++i)
             {
                 Wall wall = levelWalls[i];
                 if (wall.collisionRect.Intersects(collisionRect))
                     return true;  
             }
             for (int i = 0; i < levelItems.Count; i++)
             {
                 BaseItem item = levelItems[i];
                 if (item.collisionRect.Intersects(collisionRect))
                     return true;
             }
             return false;
         }

         void Teleport()
         {
             List<Vector2> teleportSpace = new List<Vector2>();
             teleportSpace = FindEmptySpaces();
             int randomNum = rand.Next(0, teleportSpace.Count);
             player.SetPosition = teleportSpace[randomNum];

         }
    
    }

}

        

        
       
