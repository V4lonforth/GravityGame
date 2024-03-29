﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using GravityGame.Levels;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;
using GravityGame.Effects;
using GravityGame.GameObjects.Base;
using GravityGame.Effects.PortalEffects;
using GravityGame.Effects.GravityEffects;

namespace GravityGame.Controllers
{
    public class GameController : IController
    {
        private Time time;
        private GameState gameState;

        private Drawable lastSentContour;
        private Contour lastTrajectoryContour;

        private bool launching;
        private int touchId;

        protected float currentSwitchingLevelsTime;

        private RenderTarget2D levelRenderTarget;
        private RenderTarget2D playersRenderTarget;
        private RenderTarget2D buffer;

        protected Level level;
        protected Level nextLevel;

        protected GraphicsDevice graphicsDevice;
        protected SpriteBatch spriteBatch;

        private static Effect blur;
        private static Effect portalMap;

        private static Color backgroundColor = Color.White;
        private static Color emptyColor = new Color(0, 0, 0, 0);

        private static Texture2D dottedLineCircleSprite;
        private static Vector2 dottedLineCircleSize = new Vector2(50);

        protected const float SwitchingLevelsTime = 4f;

        private const int LevelsCount = 8;

        public GameController(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;

            gameState = GameState.Playing;
            levelRenderTarget = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            playersRenderTarget = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            buffer = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);

            lastSentContour = new Drawable(dottedLineCircleSprite, Vector2.Zero, dottedLineCircleSize, 0f);
            time = new Time();
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            blur = content.Load<Effect>("Shaders/Blur");
            blur.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            blur.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));

            portalMap = content.Load<Effect>("Shaders/PortalMap");
            dottedLineCircleSprite = content.Load<Texture2D>("GameObjects/DottedLineCircle");

            Player.LoadContent(content);
            Gravity.LoadContent(content);
            Finish.LoadContent(content);
            Star.LoadContent(content);
            Portal.LoadContent(content);
            TrailDrawer.LoadContent(content, graphics);
            PortalParticlesDrawer.LoadContent(content);
            GravityParticlesDrawer.LoadContent(content);

            ParticlesDrawer<PortalParticleVertexData>.LoadContent(graphics);
            ParticlesDrawer<GravityParticleVertexData>.LoadContent(graphics);
        }

        public virtual void LoadNextLevel(int number)
        {
            nextLevel = new Level(number, new LevelsLoader().LoadInfo<LevelInfo>(number), graphicsDevice, spriteBatch);
        }

        public void StartLevel(int number)
        {
            LoadNextLevel(number);
            StartNextLevel();
        }

        public bool CheckInput(TouchLocation touch)
        {
            Vector2 position = touch.Position.ScreenToWorldPosition();
            switch (touch.State)
            {
                case TouchLocationState.Pressed:
                    if (Press(position))
                        touchId = touch.Id;
                    break;
                case TouchLocationState.Moved:
                    if (touchId == touch.Id)
                        Hold(position);
                    break;
                case TouchLocationState.Released:
                    if (touchId == touch.Id)
                        Release(position);
                    break;
            }
            return false;
        }

        protected bool Press(Vector2 position)
        {
            if (!launching && gameState == GameState.Playing && level.LaunchArea.Contains(position))
            {
                level.CreateLaunchingPlayer(position);
                launching = true;
                return true;
            }
            return false;
        }
        protected bool Hold(Vector2 position)
        {
            if (launching)
            {
                level.SetLaunchPosition(position);
                return true;
            }
            return false;
        }
        protected bool Release(Vector2 position)
        {
            if (launching)
            {
                lastSentContour.Position = level.LaunchArea.Clamp(position);
                lastTrajectoryContour = level.Contour;

                level.Launch(position);
                launching = false;
                return true;
            }
            return false;
        }

        protected virtual void StartSwitchingLevel()
        {
            launching = false;
            gameState = GameState.SwitchingLevels;
            lastTrajectoryContour = null;
            int number = level.number + 1 > LevelsCount ? level.number : level.number + 1;
            LoadNextLevel(number);
        }
        private void StartNextLevel()
        {
            level = nextLevel;
            nextLevel = null;
            gameState = GameState.Playing;
            currentSwitchingLevelsTime = 0f;
        }

        public void Update()
        {
            level.Update();
            level.UpdateEffects();
            switch (gameState)
            {
                case GameState.Playing:
                    level.CheckGravities();
                    if (level.CheckFinish())
                        StartSwitchingLevel();
                    break;

                case GameState.SwitchingLevels:
                    currentSwitchingLevelsTime += time.FixedDeltaTime;
                    if (currentSwitchingLevelsTime >= SwitchingLevelsTime)
                    {
                        StartNextLevel();
                        break;
                    }
                    break;
            }
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPlayers(spriteBatch);
            graphicsDevice.SetRenderTarget(levelRenderTarget);
            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearWrap, transformMatrix: Screen.SceneMatrix);
            level.Draw(spriteBatch);
            if (lastTrajectoryContour != null)
            {
                lastTrajectoryContour.Draw(spriteBatch);
                lastSentContour.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(playersRenderTarget, Screen.ScreenRect, Color.White);
            spriteBatch.End();

            switch (gameState)
            {
                case GameState.Playing:
                    DrawPostEffect(spriteBatch, levelRenderTarget);
                    break;

                case GameState.SwitchingLevels:
                    graphicsDevice.SetRenderTarget(buffer);
                    graphicsDevice.Clear(backgroundColor);
                    spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: Screen.SceneMatrix);
                    nextLevel.Draw(spriteBatch);
                    spriteBatch.End();

                    DrawPostEffect(spriteBatch, levelRenderTarget, currentSwitchingLevelsTime / SwitchingLevelsTime);
                    DrawPostEffect(spriteBatch, buffer, currentSwitchingLevelsTime / SwitchingLevelsTime - 1);
                    break;
            }
        }
        private void DrawPlayers(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(playersRenderTarget);
            graphicsDevice.Clear(emptyColor);
            level.DrawPlayers(spriteBatch);

            graphicsDevice.SetRenderTarget(buffer);
            graphicsDevice.Clear(emptyColor);
            portalMap.Parameters["portalMap"].SetValue(level.portalMap);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: portalMap);
            spriteBatch.Draw(playersRenderTarget, Screen.ScreenRect, Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(playersRenderTarget);
            graphicsDevice.Clear(emptyColor);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(buffer, Screen.ScreenRect, Color.White);
            spriteBatch.End();

            level.DrawPlayers(spriteBatch);
        }
        private void DrawPostEffect(SpriteBatch spriteBatch, RenderTarget2D renderTarget, float offsetX = 0)
        {
            graphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: blur);
            spriteBatch.Draw(renderTarget, new Rectangle(new Point(0, (int)(offsetX * Screen.ScreenRect.Size.Y)), Screen.ScreenRect.Size), Color.White);
            spriteBatch.End();
        }
    }
}