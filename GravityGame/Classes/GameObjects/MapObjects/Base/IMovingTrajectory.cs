using Microsoft.Xna.Framework;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public interface IMovingTrajectory
    {
        Vector2 GetPosition(float time);
    }
}