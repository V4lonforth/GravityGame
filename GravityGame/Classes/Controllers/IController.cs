using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GravityGame.Controllers
{
    public interface IController
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
        bool CheckTouch(TouchLocation touch);
    }
}