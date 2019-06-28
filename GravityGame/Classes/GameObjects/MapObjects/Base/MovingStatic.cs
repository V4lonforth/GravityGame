using Microsoft.Xna.Framework;

namespace GravityGame.GameObjects.MapObjects.Base
{
    public class MovingStatic : IMovingTrajectory
    {
        private Vector2 position;

        public MovingStatic(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetPosition(float deltaTime)
        {
            return position;
        }
    }
}