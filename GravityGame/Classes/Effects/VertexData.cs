using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityGame.Effects
{
    struct TrailVertexData : IVertexType
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public float EndTime;

        public TrailVertexData(Vector2 startPosition, Vector2 endPosition, float endTime)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            EndTime = endTime;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
            new VertexElement(sizeof(float) * 4, VertexElementFormat.Single, VertexElementUsage.PointSize, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    };
}