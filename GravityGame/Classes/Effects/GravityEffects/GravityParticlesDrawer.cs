using System;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GravityGame.Effects.GravityEffects
{
    public class GravityParticlesDrawer : ParticlesDrawer<GravityParticleVertexData>
    {
        private Gravity gravity;

        private static Effect effect;

        private const float MinTriangleRadius = 3f;
        private const float MaxTriangleRadius = 6f;

        private const float MinVertexAngle = (float)Math.PI / 2f;
        private const float MaxVertexAngle = (float)Math.PI * 5f / 6f;

        private const float MinSpawnDistance = 60f;
        private const float MaxSpawnDistance = 140f;

        private const float MinCenterRotationAcceleration = 0.25f;
        private const float MaxCenterRotationAcceleration = 0.42f;

        private const float MinVertexRotationAcceleration = 0.7f;
        private const float MaxVertexRotationAcceleration = 0.9f;

        private const float MinColorMultiplier = 0.5f;
        private const float MaxColorMultiplier = 1.5f;

        private const float Acceleration = 10f;

        private const float CreationTime = 0.05f;
        private static readonly int TrianglesCount = (int)(Math.Sqrt(2 * MaxSpawnDistance / Acceleration) / CreationTime + 1);

        public GravityParticlesDrawer(Gravity gravity) : base(MinTriangleRadius, MaxTriangleRadius, MinVertexAngle, MaxVertexAngle, CreationTime, TrianglesCount, effect)
        {
            this.gravity = gravity;
        }

        public static void LoadContent(ContentManager content)
        {
            effect = content.Load<Effect>("Shaders/GravityParticles");
            effect.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));
            effect.Parameters["acceleration"].SetValue(Acceleration);
        }
        public static void UpdateEffect()
        {
            effect.Parameters["worldMatrix"].SetValue(Screen.SceneMatrix);
            effect.Parameters["currentTime"].SetValue(Time.CurrentTime);
        }

        protected override void CreateParticle()
        {
            float angle = floatRandom.NextFloat(2f * (float)Math.PI);
            float distance = floatRandom.NextFloat(MinSpawnDistance, MaxSpawnDistance);
            Vector2 position = gravity.Position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            float centerRotationAcceleration = floatRandom.NextFloat(MinCenterRotationAcceleration, MaxCenterRotationAcceleration);
            float vertexRotationAcceleration = floatRandom.NextFloat(MinVertexRotationAcceleration, MaxVertexRotationAcceleration);

            Vector2[] vertexes = CreateTriangle();
            AddVertexesData(new GravityParticleVertexData[]
            {
                new GravityParticleVertexData(vertexes[0], distance, angle, centerRotationAcceleration, vertexRotationAcceleration, Time.CurrentTime),
                new GravityParticleVertexData(vertexes[1], distance, angle, centerRotationAcceleration, vertexRotationAcceleration, Time.CurrentTime),
                new GravityParticleVertexData(vertexes[2], distance, angle, centerRotationAcceleration, vertexRotationAcceleration, Time.CurrentTime)
            });
        }

        public new void Draw()
        {
            effect.Parameters["parentPosition"].SetValue(gravity.Position);
            base.Draw();
        }
    }
}