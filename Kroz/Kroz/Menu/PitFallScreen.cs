using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kroz.Menu
{
    class PitFallScreen : GameScreen
    {
        Texture2D wallTexture;
        Texture2D PlayerTexture;
        SpriteFont spriteFont;
        Player player;

        int timesFallen = 0;
        bool falling = true;
        String message = "YOU HIT THE BOTTOM AND DIE!";
        Vector2 messageSize = Vector2.Zero;
        Vector2 fallDirection = new Vector2(0, 1);
        List<string> pitMap = new List<string>();

         public PitFallScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            this.LoadContent();
            messageSize = spriteFont.MeasureString(message);
        }

         protected override void LoadContent()
         {
             wallTexture = Game.Content.Load<Texture2D>(@"Images\wall");
             PlayerTexture = Game.Content.Load<Texture2D>(@"Images\player");
             spriteFont = Game.Content.Load<SpriteFont>("basefont");
             this.LoadPitMap();
             base.LoadContent();
         }

         public override void Update(GameTime gameTime)
         {
             base.Update(gameTime);
             if (timesFallen < 5)
             {
                 player.SetPosition = player.GetPosition + fallDirection;
                 if (player.GetPosition.Y > 384)
                 {
                     player.SetPosition = new Vector2(528, -32);
                     fallDirection.Y *= 1.5F;
                     timesFallen++;
                 }
                 player.Update(gameTime);
             }
         }

         public override void Draw(GameTime gameTime)
         {
             base.Draw(gameTime);
             //GraphicsDevice.Clear(Color.Black);
             int mapX = 0;
             int mapY = 0;
             Vector2 position;

             player.Draw(gameTime, spriteBatch);

             for (int y = 0; y < pitMap.Count(); y++) 
             {
                 for (int x = 0; x < pitMap[y].Length; x++)
                 {
                     mapX = x * 16; //change this to use spriteSize at some point.
                     mapY = y * 16;
                     position.X = mapX;
                     position.Y = mapY;
                     if (pitMap[y][x] == '#')
                         spriteBatch.Draw(wallTexture, position, Color.White);

                 }
             }
             if (timesFallen > 3)
             {
                 if (falling == true)
                 {
                     pitMap.Add("#########################################################################################");
                     falling = false;
                 }
                 if(timesFallen > 4)
                    spriteBatch.DrawString(spriteFont, message, new Vector2(10, 440), Color.White);
             }
         }

        void LoadPitMap()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"lvl\pit.txt");
            while ((line = file.ReadLine()) != null)
            {
                pitMap.Add(line);
            }
            file.Close();

            int mapX = 0;
            int mapY = 0;
            Vector2 position;
            for (int y = 0; y < pitMap.Count(); y++)
            {
                for (int x = 0; x < pitMap[y].Length; x++)
                {
                    mapX = x * 16; //change this to use spriteSize at some point.
                    mapY = y * 16;
                    position.X = mapX;
                    position.Y = mapY;
                    if (pitMap[y][x] == 'P')
                    {
                        player = new Player(PlayerTexture, position, new Point(16, 16), 0, Vector2.Zero, 0, 0);
                        return;
                    }

                }
            }

        }

        

    }
}
