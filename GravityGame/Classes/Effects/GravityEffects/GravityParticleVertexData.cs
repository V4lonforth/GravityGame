using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityGame.Effects.GravityEffects
{
    public struct GravityParticleVertexData : IVertexType
    {
        public Vector2 VertexPosition;
        public float CenterDistance;
        public float CenterAngle;
        public float CenterRotationAcceleration;
        public float VertexRotationAcceleration;
        public float Time;

        public GravityParticleVertexData(Vector2 vertexPosition, float centerDistance, float centerAngle, float centerRotationAcceleration, float vertexRotationAcceleration, float time)
        {
            VertexPosition = vertexPosition;
            CenterDistance = centerDistance;
            CenterAngle = centerAngle;
            CenterRotationAcceleration = centerRotationAcceleration;
            VertexRotationAcceleration = vertexRotationAcceleration;
            Time = time;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector2, VertexElementUsage.Position,  0),
            new VertexElement(sizeof(float) * 2, VertexElementFormat.Single,  VertexElementUsage.PointSize, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Single,  VertexElementUsage.PointSize, 1),
            new VertexElement(sizeof(float) * 4, VertexElementFormat.Single,  VertexElementUsage.PointSize, 2),
            new VertexElement(sizeof(float) * 5, VertexElementFormat.Single,  VertexElementUsage.PointSize, 3),
            new VertexElement(sizeof(float) * 6, VertexElementFormat.Single,  VertexElementUsage.PointSize, 4)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }
}