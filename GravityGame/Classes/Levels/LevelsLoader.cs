using System;
using System.IO;
using System.Xml.Serialization;

namespace GravityGame.Levels
{
    public class LevelsLoader
    {
        const string infoPath = "Content/Levels/";

        public T LoadInfo<T>(int number)
        {
            using (Stream stream = Microsoft.Xna.Framework.Game.Activity.Assets.Open(infoPath + "Level" + number.ToString() + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                T tInfo = (T)xmlSerializer.Deserialize(stream);
                return tInfo;
            }
        }

        public void Save<T>(int number, T data)
        {
            using (StreamWriter stream = new StreamWriter(infoPath + "Level" + number.ToString() + ".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stream, data);
            }
        }
    }
}