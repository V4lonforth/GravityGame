using GravityGame.GameObjects.MapObjects;

namespace GravityGame.GameObjects.Base
{
    public interface ICollider
    {
        void Collide(Player player);
    }
}