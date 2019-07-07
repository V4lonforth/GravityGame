using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GravityGame.GameObjects.Base;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;
using GravityGame.Effects;
using GravityGame.Effects.PortalEffects;
using GravityGame.Effects.GravityEffects;
using GravityGame.Levels;
using GravityGame.Controllers;
using LevelConstructor.Levels;

namespace LevelConstructor.Controllers
{
    public class GameController
    {
        private MouseState oldState;

        private GameState gameState;
        private Level level;
        private Level nextLevel;

        private Drawable lastSentContour;
        private Contour lastTrajectoryContour;

        private Player launchingPlayerObject;
        private List<IGameObject> players;

        private bool launching;

        private float currentSwitchingLevelsTime;

        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        private RenderTarget2D levelRenderTarget;
        private RenderTarget2D playersRenderTarget;
        private RenderTarget2D buffer;

        private DateTime lastLevelChange;

        private static Effect blur;
        private static Effect portalMap;

        private static Color backgroundColor = Color.White;
        private static Color emptyColor = new Color(0, 0, 0, 0);

        private static Texture2D dottedLineCircleSprite;
        private static Vector2 dottedLineCircleSize = new Vector2(50);

        private const float SwitchingLevelsTime = 0f;
        private const float StartRadius = 300f;

        private const int LevelsCount = 8;

        public GameController(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;

            level = new Level(0, new WindowsLevelsLoader().LoadInfo<LevelInfo>(), graphicsDevice, spriteBatch);
            lastLevelChange = WindowsLevelsLoader.GetFileChangedTime("TestLevel");
            gameState = GameState.Playing;
            players = new List<IGameObject>();
            levelRenderTarget = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            playersRenderTarget = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            buffer = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);

            lastSentContour = new Drawable(dottedLineCircleSprite, Vector2.Zero, dottedLineCircleSize, 0f);
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
            Portal.LoadContent(content);
            Star.LoadContent(content);
            TrailDrawer.LoadContent(content, graphics);
            PortalParticlesDrawer.LoadContent(content);
            GravityParticlesDrawer.LoadContent(content);

            ParticlesDrawer<PortalParticleVertexData>.LoadContent(graphics);
            ParticlesDrawer<GravityParticleVertexData>.LoadContent(graphics);
        }

        public bool CheckTouch(MouseState state)
        {
            Vector2 position = state.Position.ToVector2().ScreenToWorldPosition();
            if (state.LeftButton == ButtonState.Pressed && (oldState == null || oldState.LeftButton != ButtonState.Pressed))
            {
                if (!launching && gameState == GameState.Playing && (level.StartPosition - position).LengthSquared() <= StartRadius * StartRadius)
                {
                    launching = true;
                    launchingPlayerObject = new Player(position);
                }
            }
            else if (state.LeftButton == ButtonState.Pressed)
            {
                if (launching)
                {
                    launchingPlayerObject.SetStartingPosition(position, level.StartPosition, level);
                }
            }
            else if (oldState.LeftButton == ButtonState.Pressed)
            {
                if (launching)
                {
                    lastSentContour.Position = position;
                    lastTrajectoryContour = launchingPlayerObject.Contour;

                    launchingPlayerObject.SetStartingPosition(position, level.StartPosition, level);
                    launchingPlayerObject.Launch(level.StartPosition - position);
                    players.Add(launchingPlayerObject);
                    launchingPlayerObject = null;
                    launching = false;
                }
            }
            oldState = state;
            return false;
        }

        private void StartSwitchngLevel()
        {
            launchingPlayerObject = null;
            launching = false;
            gameState = GameState.SwitchingLevels;
            nextLevel = new Level(0, new WindowsLevelsLoader().LoadInfo<LevelInfo>(), graphicsDevice, spriteBatch);
            lastTrajectoryContour = null;
        }
        private void SwitchLevel()
        {
            level = nextLevel;
            nextLevel = null;
            players.Clear();
            gameState = GameState.Playing;
            currentSwitchingLevelsTime = 0f;
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

        public void Update()
        {
            if (CheckChanges())
            {
                StartSwitchngLevel();
            }
            TrailDrawer.UpdateEffect();
            PortalParticlesDrawer.UpdateEffect();
            GravityParticlesDrawer.UpdateEffect();
            Time.Update();
            UpdatePlayerObjects();
            switch (gameState)
            {
                case GameState.Playing:
                    if (level.FinishObject.CheckCollision(players))
                        StartSwitchngLevel();
                    break;

                case GameState.SwitchingLevels:
                    currentSwitchingLevelsTime += Time.FixedDeltaTime;
                    if (currentSwitchingLevelsTime >= SwitchingLevelsTime)
                    {
                        SwitchLevel();
                        break;
                    }
                    break;
            }
        }
        
        private void UpdatePlayerObjects()
        {
            foreach (Player player in players)
            {
                level.UpdatePlayer(player);
                player.Update();
            }

            level.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPlayers(spriteBatch);
            graphicsDevice.SetRenderTarget(levelRenderTarget);
            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.LinearWrap, transformMatrix: Screen.SceneMatrix);
            level.Draw(spriteBatch);
            if (launchingPlayerObject != null)
                launchingPlayerObject.Draw(spriteBatch);
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
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: Screen.SceneMatrix);
            foreach (Player player in players)
                if (player.State != PlayerState.Free)
                    player.Draw(spriteBatch);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(buffer);
            graphicsDevice.Clear(emptyColor);
            portalMap.Parameters["portalMap"].SetValue(level.PortalMap);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: portalMap);
            spriteBatch.Draw(playersRenderTarget, Screen.ScreenRect, Color.White);
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(playersRenderTarget);
            graphicsDevice.Clear(emptyColor);
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(buffer, Screen.ScreenRect, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: Screen.SceneMatrix);
            foreach (Player player in players)
                if (player.State == PlayerState.Free)
                    player.Draw(spriteBatch);
            spriteBatch.End();
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