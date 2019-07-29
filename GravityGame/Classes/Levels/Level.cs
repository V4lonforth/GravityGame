using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.MapObjects;
using GravityGame.GameObjects.Base;
using GravityGame.Utils;
using GravityGame.Effects;

namespace GravityGame.Levels
{
    public class Level
    {
        private Player launchingPlayerObject;
        private List<IGameObject> players;
        private Time time;

        private Gravity[] gravityObjects;
        private Portal[] portals;
        private Finish finishObject;
        private Star[] stars;

        private List<IGameObject> gameObjects;
        private List<ICollider> colliders;

        private int trajectorySections;
        private float trajectoryLength;
        private float launchForce;

        public Contour Contour { get; private set; }
        public Vector2 startPosition;
        public int number;
        public RenderTarget2D portalMap;

        public Level(int number, LevelInfo info, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.number = number;

            startPosition = info.PlayerPosition;
            trajectorySections = info.TrajectorySections;
            trajectoryLength = info.TrajectoryLength;
            launchForce = info.LaunchForce;

            finishObject = new Finish(info.Finish.Trajectory.GetMovingTrajectory(), info.Finish.Size, 0f, info.Finish.Color);
            gravityObjects = info.GetGravityObjects();
            stars = info.GetStars();
            portals = info.GetPortals();

            gameObjects = new List<IGameObject>();
            gameObjects.Add(finishObject);
            gameObjects.AddRange(gravityObjects);
            gameObjects.AddRange(stars);
            gameObjects.AddRange(portals);

            colliders = new List<ICollider>();
            colliders.AddRange(portals);

            time = new Time();
            players = new List<IGameObject>();
            Contour = new Contour(trajectorySections);

            DrawPortalMap(graphicsDevice, spriteBatch);
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
            foreach (Star star in stars)
                star.RemovePlayer(player);
        }

        public void CreateLaunchingPlayer(Vector2 launchPosition)
        {
            launchingPlayerObject = new Player(launchPosition);
            Contour = new Contour(trajectorySections);
        }
        public void SetLaunchPosition(Vector2 launchPosition)
        {
            launchingPlayerObject.SetStartPosition(startPosition, launchPosition, launchForce);
            launchingPlayerObject.Position = launchPosition;
            Contour.CreateContour(GetTrajectory(launchingPlayerObject, launchPosition));
        }
        public void Launch(Vector2 launchPosition)
        {
            Contour.CreateContour(GetTrajectory(launchingPlayerObject, launchPosition));
            launchingPlayerObject.Launch(startPosition, launchPosition, launchForce);
            AddPlayer(launchingPlayerObject);
            launchingPlayerObject = null;
        }

        private List<List<Vector2>> GetTrajectory(Player player, Vector2 launchPosition)
        {
            Time tempTime = time.Copy();
            Vector2 tempPosition = player.Position;

            player.SetStartPosition(startPosition, launchPosition, launchForce);

            float distance = 0f;
            int sectionNumber = 0;
            Vector2 lastPosition = startPosition;

            List<List<Vector2>> trajectory = new List<List<Vector2>>() { new List<Vector2>() { lastPosition } };
            int trajectoryNumber = 0;

            while (distance < trajectoryLength && sectionNumber < trajectorySections)
            {
                CheckStars();
                tempTime.Update();
                UpdateGameObjects(tempTime);
                UpdatePlayer(player, tempTime);

                if (player.Teleported)
                {
                    trajectoryNumber++;
                    trajectory.Add(new List<Vector2>() { player.Position });
                }
                else
                {
                    distance += (player.Position - lastPosition).Length();
                    trajectory[trajectoryNumber].Add(player.Position);
                }
                lastPosition = player.Position;
                sectionNumber++;
            }

            player.Position = tempPosition;
            return trajectory;
        }

        private void CheckStars()
        {
            foreach (IGameObject player in players)
                foreach (Star star in stars)
                    star.TryGainStar(player);
        }

        private void CalculateForce(Player player, Time time)
        {
            Vector2 force = Vector2.Zero;
            foreach (Gravity gravityObject in gravityObjects)
                force += gravityObject.CalculateForce(player.Position);
            player.Velocity += force * time.FixedDeltaTime;
        }
        private void Collide(Player player)
        {
            foreach (ICollider collider in colliders)
                collider.Collide(player);
        }

        public void UpdatePlayer(Player player, Time time)
        {
            CalculateForce(player, time);
            Collide(player);
            player.Update(time);
        }

        public bool CheckFinish()
        {
            return finishObject.CheckCollision(players);
        }

        public void Update()
        {
            time.Update();
            UpdateGameObjects(time);
            UpdatePlayerObjects(time);
            CheckStars();
        }

        private void UpdatePlayerObjects(Time time)
        {
            foreach (Player player in players)
                UpdatePlayer(player, time);
        }

        private void UpdateGameObjects(Time time)
        {
            foreach (IGameObject gameObject in gameObjects)
                gameObject.Update(time);
        }

        public void UpdateEffects()
        {
            foreach (IGameObject gameObject in gameObjects)
                gameObject.UpdateEffects(time);
            foreach (IGameObject gameObject in players)
                gameObject.UpdateEffects(time);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IGameObject gameObject in gameObjects)
                gameObject.Draw(spriteBatch);
            if (launchingPlayerObject != null)
            {
                launchingPlayerObject.Draw(spriteBatch);
                Contour.Draw(spriteBatch);
            }
        }

        public void DrawPlayers(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: Screen.SceneMatrix);
            foreach (Player player in players)
                if (player.IsOutsidePortal)
                    player.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawPortalMap(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            portalMap = new RenderTarget2D(graphicsDevice, Screen.ScreenSize.X, Screen.ScreenSize.Y);
            graphicsDevice.SetRenderTarget(portalMap);
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin(transformMatrix: Screen.SceneMatrix);
            foreach (Portal portal in portals)
                portal.DrawMap(spriteBatch);
            spriteBatch.End();
        }
    }
}