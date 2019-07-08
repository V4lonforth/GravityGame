using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GravityGame.Controllers;
using GravityGame.GUI;
using GravityGame.Utils;

namespace GravityGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameController gameController;
        private GUIController guiController;

        private List<IController> controllers;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/";

            graphics.PreferredBackBufferWidth = Screen.ScreenSize.X;
            graphics.PreferredBackBufferHeight = Screen.ScreenSize.Y;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            gameController = new GameController(GraphicsDevice, spriteBatch);
            gameController.StartLevel(1);
            guiController = new GUIController();

            controllers = new List<IController>()
            {
                guiController,
                gameController
            };
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameController.LoadContent(Content, GraphicsDevice);
            GUIController.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    Exit();


            CheckTouches();
            foreach (IController controller in controllers)
                controller.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            for (int i = controllers.Count - 1; i >= 0; i--)
                controllers[i].Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void CheckTouches()
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation touch in touchCollection)
            {
                foreach (IController controller in controllers)
                    if (controller.CheckInput(touch))
                        break;
            }
        }
    }
}
