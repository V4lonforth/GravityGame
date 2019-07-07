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
        public int Number { get; }
        public Vector2 StartPosition { get; }
        public List<Gravity> GravityObjects { get; }
        public List<Portal> Portals { get; }
        public Finish FinishObject { get; }

        public List<IGameObject> GameObjects { get; }
        public List<ICollider> Colliders { get; }

        public RenderTarget2D PortalMap { get; private set; }

        public Level(int number, LevelInfo info, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Number = number;

            GameObjects = new List<IGameObject>();
            Colliders = new List<ICollider>();

            StartPosition = info.PlayerPosition;
            FinishObject = new Finish(GetMovingTrajectory(info.Finish.Trajectory), info.Finish.Size, 0f, info.Finish.Color);
            GameObjects.Add(FinishObject);
            GravityObjects = new List<Gravity>(info.GravityObjects == null ? 0 : info.GravityObjects.Length);
            Portals = new List<Portal>(info.Portals == null ? 0 : info.Portals.Length * 2);
            for (int i = 0; i < GravityObjects.Capacity; i++)
            {
                Gravity gravity = new Gravity(info.GravityObjects[i].GravityPower, GetMovingTrajectory(info.GravityObjects[i].MapObject.Trajectory), info.GravityObjects[i].MapObject.Size, 0f);
                GravityObjects.Add(gravity);
                GameObjects.Add(gravity);
            }

            for (int i = 0; i < Portals.Capacity / 2; i++)
            {
                Portal first = new Portal(GetMovingTrajectory(info.Portals[i].FirstPortal.Trajectory), info.Portals[i].FirstPortal.Size, info.Portals[i].FirstPortal.Rotation / 180f * (float)Math.PI, info.Portals[i].FirstPortal.Color);
                Portal second = new Portal(GetMovingTrajectory(info.Portals[i].SecondPortal.Trajectory), info.Portals[i].SecondPortal.Size, info.Portals[i].SecondPortal.Rotation / 180f * (float)Math.PI, info.Portals[i].SecondPortal.Color);
                first.NextPortal = second;
                second.NextPortal = first;
                Portals.Add(first);
                Portals.Add(second);
                GameObjects.Add(first);
                GameObjects.Add(second);
                Colliders.Add(first);
                Colliders.Add(second);
            }

            DrawPortalMap(graphicsDevice, spriteBatch);
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

        private void CalculateForce(Player player)
        {
            Vector2 force = Vector2.Zero;
            foreach (Gravity gravityObject in GravityObjects)
                force += gravityObject.CalculateForce(player.Position);
            player.Velocity += force * Time.FixedDeltaTime;
        }
        private void Collide(Player player)
        {
            foreach (ICollider collider in Colliders)
                collider.Collide(player);
        }

        public void UpdatePlayer(Player player)
        {
            CalculateForce(player);
            Collide(player);
        }

        public void Update()
        {
            foreach (IGameObject gameObject in GameObjects)
                gameObject.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IGameObject gameObject in GameObjects)
                gameObject.Draw(spriteBatch);
        }
    }
}