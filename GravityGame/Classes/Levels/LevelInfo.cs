using System;
using Microsoft.Xna.Framework;
using GravityGame.Levels.MapObjects;
using GravityGame.GameObjects.MapObjects;

namespace GravityGame.Levels
{
    [Serializable]
    public class LevelInfo
    {
        public Vector2 PlayerPosition;
        public MapObjectInfo Finish;
        public GravityInfo[] GravityObjects;
        public PortalsInfo[] Portals;
        public MapObjectInfo[] Stars;

        public float TrajectoryLength;
        public int TrajectorySections;
        public float LaunchForce;

        public LevelInfo()
        {
            TrajectoryLength = 1000f;
            TrajectorySections = 120;
            LaunchForce = 0.5f;
        }

        public Gravity[] GetGravityObjects()
        {
            Gravity[] gravityObjects = new Gravity[GravityObjects == null ? 0 : GravityObjects.Length];
            for (int i = 0; i < gravityObjects.Length; i++)
                gravityObjects[i] = GravityObjects[i].GetGravity();
            return gravityObjects;
        }

        public Star[] GetStars()
        {
            Star[] stars = new Star[Stars == null ? 0 : Stars.Length];
            for (int i = 0; i < stars.Length; i++)
            {
                Star star = new Star(Stars[i].Trajectory.GetMovingTrajectory(), Stars[i].Size, Stars[i].Rotation, Stars[i].Color);
                stars[i] = star;
            }
            return stars;
        }

        public Portal[] GetPortals()
        {
            Portal[] portals = new Portal[Portals == null ? 0 : Portals.Length * 2];
            for (int i = 0; i < portals.Length / 2; i++)
                Portals[i].GetPortals(out portals[i * 2], out portals[i * 2 + 1]);
            return portals;
        }
    }
}