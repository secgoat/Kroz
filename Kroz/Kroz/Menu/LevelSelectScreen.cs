using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using FuchsGUI;

namespace Kroz.Menu
{
    class LevelSelectScreen : GameScreen
    {

        KeyboardState keyboardState;
        MouseState mouseState;
        //FUSCHS GUI COMPONENTS
        Form lvlLoadForm;
        TextBox txtLevelSelect;
        Button exitButton;
        Button loadButton;
        //menu components
        Texture2D image;
        SpriteFont spriteFont;
        Rectangle imageRectangle;

        int levelToLoad;

        public Button Exit { get { return exitButton; } }
        public Button Load { get { return loadButton; } }
        public int Level { get { return levelToLoad; } }

        
        public LevelSelectScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image)
            : base(game, spriteBatch)
        {
            this.image = image;
            this.spriteFont = spriteFont;
            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            this.LoadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Texture2D txtButton = game.Content.Load<Texture2D>(@"GUI\texButton");
            Texture2D txtTextBox = game.Content.Load<Texture2D>(@"GUI\texTextBox");
            Texture2D txtForm = game.Content.Load<Texture2D>(@"GUI\texForm");
            lvlLoadForm = new Form("LoadForm", "levels: 0 - 20 ", 
                new Rectangle((Game.Window.ClientBounds.Width / 2 - 75), (Game.Window.ClientBounds.Height / 2 - 45),
                    150, 90), txtForm, spriteFont, Color.Black);
            txtLevelSelect = new TextBox("level", " ", 2, "0123456789-.", new Rectangle(10, 10, 50, 30), txtTextBox, spriteFont, Color.Black);
            exitButton = new Button("Exit", "Exit", new Rectangle(75,60,60,20), txtButton, spriteFont, Color.Black);
            loadButton = new Button("Load", "Load", new Rectangle(10, 60, 60, 20), txtButton, spriteFont, Color.Black);

            lvlLoadForm.AddControl(txtLevelSelect);
            lvlLoadForm.AddControl(exitButton);
            lvlLoadForm.AddControl(loadButton);
            
            loadButton.onClick += new EHandler(LoadButtonClicked);
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            lvlLoadForm.Update(mouseState, keyboardState);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Draw(image, imageRectangle, Color.White);
            lvlLoadForm.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        void LoadButtonClicked(Control sender)
        {
            levelToLoad = Convert.ToInt32(txtLevelSelect.Text);
        }
    }
}
