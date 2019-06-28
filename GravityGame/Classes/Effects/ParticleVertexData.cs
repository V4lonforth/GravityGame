using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityGame.Effects
{
    public struct ParticleVertexData : IVertexType
    {
        public Vector2 Position;
        public Vector2 LocalPosition;
        public Vector2 Speed;
        public float RotationSpeed;
        public float Time;
        public Color Color;

        public ParticleVertexData(Vector2 position, Vector2 localPosition, Vector2 speed, float rotationSpeed, float time, Color color)
        {
            LocalPosition = localPosition;
            Position = position;
            Speed = speed;
            RotationSpeed = rotationSpeed;
            Time = time;
            Color = color;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
            new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.Position, 2),
            new VertexElement(sizeof(float) * 6, VertexElementFormat.Single, VertexElementUsage.PointSize, 0),
            new VertexElement(sizeof(float) * 7, VertexElementFormat.Single, VertexElementUsage.PointSize, 1),
            new VertexElement(sizeof(float) * 8, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }
}