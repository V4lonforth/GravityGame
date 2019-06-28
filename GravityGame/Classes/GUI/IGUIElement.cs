using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using GravityGame.GameObjects.Base;

namespace GravityGame.GUI
{
    public interface IGUIElement : IGameObject
    {
        int Id { get; }
        Vector2 LocalPosition { get; set; }

        bool CheckTouch(TouchLocation touch);
    }
}