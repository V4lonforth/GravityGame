using System;
using Microsoft.Xna.Framework;
using GravityGame.Levels.MapObjects;

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
    }
}