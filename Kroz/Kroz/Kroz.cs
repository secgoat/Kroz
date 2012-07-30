using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FuchsGUI;
using Kroz.Menu;

namespace Kroz
{
    public class Kroz : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        LevelManager levelManager;
        StartScreen startScreen;
        GameScreen activeScreen;
        LevelSelectScreen levelSelectScreen;
        PitFallScreen pitFallScreen;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        MouseState mouseState;
        MouseState oldMouseState;

        Texture2D transparentTexture;

        public Kroz()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 1056;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("BaseFont");
            transparentTexture = Content.Load<Texture2D>("transparent");
            //-----------------------------This is a neat way to have a blank texture to use for whatever
            Texture2D blankTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //---------------------------------------------------------------------------------------------

            levelManager = new LevelManager(this, spriteBatch);
            Components.Add(levelManager);
            levelManager.Hide();

            //startScreen = new StartScreen(this, spriteBatch, spriteFont, Content.Load<Texture2D>("alienmetal"));
            startScreen = new StartScreen(this, spriteBatch, spriteFont, blankTexture);
            Components.Add(startScreen);
            startScreen.Hide();

           levelSelectScreen = new LevelSelectScreen(this, spriteBatch, spriteFont, transparentTexture );
            Components.Add(levelSelectScreen);
            levelSelectScreen.Hide();

            
            pitFallScreen = new PitFallScreen(this, spriteBatch, spriteFont, blankTexture);
            Components.Add(pitFallScreen);
            pitFallScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            IsMouseVisible = true;

            levelSelectScreen.Exit.onClick += new EHandler(HandleSelectLevelScreen);
            levelSelectScreen.Load.onClick += new EHandler(HandleSelectLevelScreen);

            levelManager.PitFall += new LevelManager.PitEvent(StartPitFallScreen);
        }

        protected override void UnloadContent()
        {
        }

        

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (activeScreen == startScreen)
            {
                HandleStartScreen();
            }

            if (activeScreen == levelSelectScreen)
            {
                HandleSelectLevelScreen(null);
            }

            if (activeScreen == pitFallScreen)
            {
                HandlePitFallScreen();
            }

            oldKeyboardState = keyboardState;
            oldMouseState = mouseState;
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
            
        }

        private bool CheckKey(Keys key)
        {
            return keyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key);
        }

        private void HandleStartScreen()
        {
            if (CheckKey(Keys.Enter))
            {
                if (startScreen.SelectedIndex == 0)
                {
                    activeScreen.Hide();
                    activeScreen = levelManager;
                    activeScreen.Show();
                }
                if (startScreen.SelectedIndex == 1)
                {
                    activeScreen.Hide();
                    activeScreen = levelSelectScreen;
                    oldKeyboardState = new KeyboardState(); //use this to zero out oldkeyboard 
                    //state otherwise it selects first itemon next menu
                    activeScreen.Show();
                }
                if (startScreen.SelectedIndex == 2)
                {
                    this.Exit();
                }
            }
        }

        private void HandleSelectLevelScreen(Control sender)
        {
            if (sender == null)
                return;
            if (sender.Name == "Exit")
            {
                activeScreen.Hide();
                activeScreen = startScreen;
                activeScreen.Show();
            }
            if (sender.Name == "Load")
            {
                activeScreen.Hide();
                levelManager.Level = levelSelectScreen.Level;
                activeScreen = levelManager;
                activeScreen.Show();
            }
        }

        public void HandlePitFallScreen()
        {
            if (CheckKey(Keys.Y))
            {
                activeScreen.Hide();
                activeScreen = startScreen;
                activeScreen.Show();
            }
            if (CheckKey(Keys.N))
            {
                this.Exit();
            }
        }
        
        public void StartPitFallScreen()
        {
            activeScreen.Hide();
            activeScreen = pitFallScreen;
            activeScreen.Show();



        }

    }
}
