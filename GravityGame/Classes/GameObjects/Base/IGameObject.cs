using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.Utils;

namespace GravityGame.GameObjects.Base
{
    public interface IGameObject
    {
        Vector2 Position { get; set; }
        Vector2 Center { get; }
        Vector2 Size { get; set; }
        float Radius { get; }
        float Rotation { get; set; }
        Color Color { get; set; }

        bool CheckCollision(IGameObject drawable);
        bool CheckCollision(List<IGameObject> drawables);

        void Draw(SpriteBatch spriteBatch);
        void UpdateEffects(Time time);
        void Update(Time time);
    }
}