using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GravityGame.Levels;
using GravityGame.Controllers;
using LevelConstructor.Levels;

namespace LevelConstructor.Controllers
{
    public class WindowsGameController : GameController
    {
        private MouseState oldState;

        private DateTime lastLevelChange;

        private const int LevelsCount = 8;

        public WindowsGameController(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) : base(graphicsDevice, spriteBatch)
        {
        }

        public override void LoadNextLevel(int number)
        {
            nextLevel = new Level(number, new WindowsLevelsLoader().LoadInfo<LevelInfo>(), graphicsDevice, spriteBatch);
            lastLevelChange = WindowsLevelsLoader.GetFileChangedTime("TestLevel");
        }

        public bool CheckInput(MouseState state)
        {
            Vector2 position = state.Position.ToVector2().ScreenToWorldPosition();
            if (state.LeftButton == ButtonState.Pressed && (oldState == null || oldState.LeftButton != ButtonState.Pressed))
                Press(position);
            else if (state.LeftButton == ButtonState.Pressed)
                Hold(position);
            else if (oldState.LeftButton == ButtonState.Pressed)
                Release(position);

            oldState = state;
            return false;
        }

        protected override void StartSwitchingLevel()
        {
            base.StartSwitchingLevel();
            nextLevel = new Level(0, new WindowsLevelsLoader().LoadInfo<LevelInfo>(), graphicsDevice, spriteBatch);
        }

        public new void Update()
        {
            if (CheckChanges())
                StartLevel(0);
            base.Update();
        }

        public bool CheckChanges()
        {
            DateTime levelChange = WindowsLevelsLoader.GetFileChangedTime("TestLevel");
            if (levelChange != lastLevelChange)
            {
                lastLevelChange = levelChange;
                return true;
            }
            return false;
        }
    }
}