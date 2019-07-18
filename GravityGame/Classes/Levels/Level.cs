using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Levels.MapObjects;
using GravityGame.GameObjects.MapObjects.Base;
using GravityGame.GameObjects.Base;
using GravityGame.Utils;

namespace GravityGame.Levels
{
    public class Level
    {
        public List<IGameObject> players;
        public Time Time { get; }

        public int Number { get; }
        public Vector2 StartPosition { get; }
        public Gravity[] GravityObjects { get; }
        public Portal[] Portals { get; }
        public Finish FinishObject { get; }
        public Star[] Stars { get; }

        public List<IGameObject> GameObjects { get; }
        public List<ICollider> Colliders { get; }

        public RenderTarget2D PortalMap { get; private set; }

        public Level(int number, LevelInfo info, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Time = new Time();
            players = new List<IGameObject>();
            Number = number;

            GameObjects = new List<IGameObject>();
            Colliders = new List<ICollider>();

            StartPosition = info.PlayerPosition;
            FinishObject = new Finish(GetMovingTrajectory(info.Finish.Trajectory), info.Finish.Size, 0f, info.Finish.Color);
            GameObjects.Add(FinishObject);
            GravityObjects = new Gravity[info.GravityObjects == null ? 0 : info.GravityObjects.Length];
            Stars = new Star[info.Stars == null ? 0 : info.Stars.Length];
            Portals = new Portal[info.Portals == null ? 0 : info.Portals.Length * 2];

            for (int i = 0; i < GravityObjects.Length; i++)
            {
                Gravity gravity = new Gravity(info.GravityObjects[i].GravityPower, GetMovingTrajectory(info.GravityObjects[i].MapObject.Trajectory), info.GravityObjects[i].MapObject.Size, 0f);
                GravityObjects[i] = gravity;
                GameObjects.Add(gravity);
            }

            for (int i = 0; i < Stars.Length; i++)
            {
                Star star = new Star(GetMovingTrajectory(info.Stars[i].Trajectory), info.Stars[i].Size, info.Stars[i].Rotation, info.Stars[i].Color);
                Stars[i] = star;
                GameObjects.Add(star);
            }

            for (int i = 0; i < Portals.Length / 2; i++)
            {
                Portal first = new Portal(GetMovingTrajectory(info.Portals[i].FirstPortal.Trajectory), info.Portals[i].FirstPortal.Size, info.Portals[i].FirstPortal.Rotation / 180f * (float)Math.PI, info.Portals[i].FirstPortal.Color);
                Portal second = new Portal(GetMovingTrajectory(info.Portals[i].SecondPortal.Trajectory), info.Portals[i].SecondPortal.Size, info.Portals[i].SecondPortal.Rotation / 180f * (float)Math.PI, info.Portals[i].SecondPortal.Color);
                first.NextPortal = second;
                second.NextPortal = first;
                Portals[i * 2] = first;
                Portals[i * 2 + 1] = second;
                GameObjects.Add(first);
                GameObjects.Add(second);
                Colliders.Add(first);
                Colliders.Add(second);
            }

            DrawPortalMap(graphicsDevice, spriteBatch);
        }

        private IMovingTrajectory GetMovingTrajectory(MovingTrajectoryInfo trajectory)
        {
            IMovingTrajectory movingTrajectory = null;
            switch (trajectory.MovingType)
            {
                case MovingType.Circle:
                    movingTrajectory = new MovingCircle(trajectory.Center, trajectory.Radius, trajectory.CirclePeriod, trajectory.CircleAngle);
                    break;
                case MovingType.Linear:
                    movingTrajectory = new MovingLine(trajectory.FirstPosition, trajectory.SecondPosition, trajectory.LinearPeriod, trajectory.LinearTime);
                    break;
                case MovingType.Static:
                    movingTrajectory = new MovingStatic(trajectory.Position);
                    break;
            }
            return movingTrajectory;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            GameObjects.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            GameObjects.Remove(player);
        }

        private void CheckStars()
        {
            foreach (IGameObject player in players)
                foreach (Star star in Stars)
                    star.TryGainStar(player);
        }

        private void CalculateForce(Player player, Time time)
        {
            Vector2 force = Vector2.Zero;
            foreach (Gravity gravityObject in GravityObjects)
                force += gravityObject.CalculateForce(player.Position);
            player.Velocity += force * time.FixedDeltaTime;
        }
        private void Collide(Player player)
        {
            foreach (ICollider collider in Colliders)
                collider.Collide(player);
        }

        public void UpdatePlayer(Player player)
        {
            CalculateForce(player, Time);
            Collide(player);
        }

        public bool CheckFinish()
        {
            return FinishObject.CheckCollision(players);
        }

        public void UpdatePlayerObjects()
        {
            foreach (Player player in players)
            {
                UpdatePlayer(player);
                player.Update(Time);
            }

            Update();
        }

        public void Update()
        {
            Time.Update();
            foreach (IGameObject gameObject in GameObjects)
                gameObject.Update(Time);
            CheckStars();
        }

        public void UpdateEffects()
        {
            foreach (IGameObject gameObject in GameObjects)
                gameObject.UpdateEffects(Time);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IGameObject gameObject in GameObjects)
                gameObject.Draw(spriteBatch);
        }

        public void DrawPlayers(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: Screen.SceneMatrix);
            foreach (Player player in players)
                if (player.State != PlayerState.Free)
                    player.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawPortalMap(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            PortalMap = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            graphicsDevice.SetRenderTarget(PortalMap);
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin(transformMatrix: Screen.SceneMatrix);
            foreach (Portal portal in Portals)
                portal.DrawMap(spriteBatch);
            spriteBatch.End();
        }
    }
}