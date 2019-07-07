using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LevelConstructor.Controllers;
using GravityGame.GUI;
using GravityGame.Utils;

namespace LevelConstructor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameController gameController;

        private List<IController> controllers;

        public Game1()
        {
            Screen.CreateData(new Point(540, 960));

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/";

            graphics.PreferredBackBufferWidth = Screen.ScreenSize.X;
            graphics.PreferredBackBufferHeight = Screen.ScreenSize.Y;
            graphics.ApplyChanges();
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            gameController = new GameController(GraphicsDevice, spriteBatch);

            controllers = new List<IController>()
            {
                gameController
            };
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameController.LoadContent(Content, GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            CheckTouches();
            foreach (IController controller in controllers)
                controller.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameController.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void CheckTouches()
        {
            MouseState state = Mouse.GetState();
            gameController.CheckTouch(state);
        }
    }
}
