using System;
using GravityGame.GameObjects.MapObjects;

namespace GravityGame.Levels.MapObjects
{
    [Serializable]
    public class GravityInfo
    {
        public MapObjectInfo MapObject;
        public float GravityPower;

        public Gravity GetGravity()
        {
            return new Gravity(GravityPower, MapObject.Trajectory.GetMovingTrajectory(), MapObject.Size, 0f);
        }
    }
}