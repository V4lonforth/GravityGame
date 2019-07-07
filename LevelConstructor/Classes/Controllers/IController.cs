using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelConstructor.Controllers
{
    public interface IController
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
        bool CheckTouch(MouseState state);
    }
}