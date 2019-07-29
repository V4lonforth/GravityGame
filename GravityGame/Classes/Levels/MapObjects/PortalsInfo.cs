using System;
using GravityGame.GameObjects.MapObjects;

namespace GravityGame.Levels.MapObjects
{
    [Serializable]
    public class PortalsInfo
    {
        public MapObjectInfo FirstPortal;
        public MapObjectInfo SecondPortal;

        public void GetPortals(out Portal first, out Portal second)
        {
            first = new Portal(FirstPortal.Trajectory.GetMovingTrajectory(), FirstPortal.Size, FirstPortal.Rotation / 180f * (float)Math.PI, FirstPortal.Color);
            second = new Portal(SecondPortal.Trajectory.GetMovingTrajectory(), SecondPortal.Size, SecondPortal.Rotation / 180f * (float)Math.PI, SecondPortal.Color);
            first.NextPortal = second;
            second.NextPortal = first;
        }
    }
}