using System;
using Microsoft.Xna.Framework;

namespace GravityGame.Levels.MapObjects
{
    [Serializable]
    public class MapObjectInfo
    {
        public Vector2 Size;
        public float Rotation;
        public MovingTrajectoryInfo Trajectory;
        public Color Color;
    }
}