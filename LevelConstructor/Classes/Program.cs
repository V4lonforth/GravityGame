using GravityGame;
using GravityGame.Levels;
using LevelConstructor.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;

namespace LevelConstructor
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            /*XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelInfo));
            new WindowsLevelsLoader().Save<LevelInfo>(new LevelInfo()
            {
                PlayerPosition = new Vector2(0, 700),
                Finish = new GravityGame.Levels.MapObjects.MapObjectInfo()
                {
                    Color = new Color(115, 221, 126, 255),
                    Rotation = 0f,
                    Size = new Vector2(100),
                    Trajectory = new GravityGame.Levels.MapObjects.MovingTrajectoryInfo()
                    {
                        MovingType = GravityGame.Levels.MapObjects.MovingType.Static,
                        Position = new Vector2(0, -700)
                    }
                },
                GravityObject = new GravityGame.Levels.MapObjects.GravityInfo[]
                {
                    new GravityGame.Levels.MapObjects.GravityInfo()
                    {
                        GravityPower = 110,
                        MapObject = new GravityGame.Levels.MapObjects.MapObjectInfo()
                        {
                            Color = Color.Black,
                            Rotation = 0f,
                            Size = new Vector2(80),
                            Trajectory = new GravityGame.Levels.MapObjects.MovingTrajectoryInfo()
                            {
                                MovingType = GravityGame.Levels.MapObjects.MovingType.Static
                            }
                        }
                    }
                }
            });*/
            using (var game = new Game1())
                game.Run();
        }
    }
}
