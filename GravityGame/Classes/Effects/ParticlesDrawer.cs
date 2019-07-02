using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityGame.Utils;

namespace GravityGame.Effects
{
    public abstract class ParticlesDrawer<T> where T : struct, IVertexType
    {
        protected FloatRandom floatRandom;

        private Effect effect;

        private T[] vertexesData;
        private int index;

        private float timeToCreate;
        private float creationTime;

        private float minTriangleRadius;
        private float maxTriangleRadius;

        private float minVertexAngle;
        private float maxVertexAngle;

        private int trianglesCount;
        private int vertexesCount;

        private static GraphicsDevice graphicsDevice;

        public ParticlesDrawer(float minTriangleRadius, float maxTriangleRadius, float minVertexAngle, float maxVertexAngle, float creationTime, int trianglesCount, Effect effect)
        {
            this.minTriangleRadius = minTriangleRadius;
            this.maxTriangleRadius = maxTriangleRadius;

            this.minVertexAngle = minVertexAngle;
            this.maxVertexAngle = maxVertexAngle;

            this.creationTime = creationTime;

            this.trianglesCount = trianglesCount;
            vertexesCount = this.trianglesCount * 3;

            vertexesData = new T[vertexesCount];

            this.effect = effect;

            floatRandom = new FloatRandom();
        }


        public static void LoadContent(GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
        }

        private Vector2 CreateVertex(float radius, float angle)
        {
            return Vector2.UnitX.Rotate(angle) * radius;
        }

        protected Vector2[] CreateTriangle()
        {
            float angle = 0f;
            return new Vector2[]
            {
                CreateVertex(floatRandom.NextFloat(minTriangleRadius, maxTriangleRadius), angle = angle + floatRandom.NextFloat(0, 2f * (float)Math.PI)),
                CreateVertex(floatRandom.NextFloat(minTriangleRadius, maxTriangleRadius), angle = angle + floatRandom.NextFloat(minVertexAngle, maxVertexAngle)),
                CreateVertex(floatRandom.NextFloat(minTriangleRadius, maxTriangleRadius), angle = angle + floatRandom.NextFloat(minVertexAngle, maxVertexAngle))
            };
        }
        protected void AddVertexesData(T[] vertexes)
        {
            Array.Copy(vertexes, 0, vertexesData, index, vertexes.Length);
            index = (index + vertexes.Length) % vertexesCount;
        }

        protected abstract void CreateParticle();

        public void Update()
        {
            timeToCreate -= Time.FixedDeltaTime;
            while (timeToCreate <= 0f)
            {
                CreateParticle();
                timeToCreate += creationTime;
            }
        }

        public void Draw()
        {
            effect.Techniques[0].Passes[0].Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexesData, 0, trianglesCount);
        }
    }
}