using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LevelConstructor.Controllers;
using GravityGame.Utils;
using GravityGame.Controllers;
using GravityGame.Effects;

namespace LevelConstructor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private WindowsGameController gameController;

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

            //Contour.TrajectoryLength = 5000f;
            //Contour.TrajectorySections = 600;

            gameController = new WindowsGameController(GraphicsDevice, spriteBatch);
            gameController.StartLevel(0);
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
            gameController.CheckInput(Mouse.GetState());
            gameController.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameController.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
