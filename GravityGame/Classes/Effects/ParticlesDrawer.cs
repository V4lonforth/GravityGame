using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;

namespace GravityGame.Effects
{
    public class ParticlesDrawer
    {
        private Portal portal;
        private Color color;

        private ParticleVertexData[] vertexesData;
        private int index;

        private float timeToCreate;

        private static GraphicsDevice graphicsDevice;
        private static Effect effect;

        private static float[] randomValues;
        private static int randomIndex;

        private const int randomValuesCount = 1337;

        private const float minRadius = 2f;
        private const float maxRadius = 5f;

        private const float minRotationSpeed = (float)Math.PI / 2f;
        private const float maxRotationSpeed = (float)Math.PI;

        private const float minSpeed = 4f;
        private const float maxSpeed = 21f;
        private const float maxSpeedAngle = 0.3f;

        private const float minVertexAngle = (float)Math.PI / 2f;
        private const float maxVertexAngle = (float)Math.PI * 5f / 6f;

        private const float CreatingTime = 0.022f;
        private const float LifeTime = 1.8f;

        private const float minColorMultiplier = 0.5f;
        private const float maxColorMultiplier = 1.5f;

        private const int TrianglesCount = (int)(LifeTime / CreatingTime + 1f);
        private const int VertexesCount = TrianglesCount * 3;

        public ParticlesDrawer(Portal portal, Color color)
        {
            this.portal = portal;
            this.color = color;

            vertexesData = new ParticleVertexData[VertexesCount];
        }
        public static void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            effect = content.Load<Effect>("Shaders/Particles");
            effect.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));
            effect.Parameters["duration"].SetValue(LifeTime);

            Random random = new Random();
            randomValues = new float[randomValuesCount];
            for (int i = 0; i < randomValuesCount; i++)
                randomValues[i] = (float)random.NextDouble();
        }
        public static void UpdateEffect()
        {
            effect.Parameters["worldMatrix"].SetValue(Screen.SceneMatrix);
            effect.Parameters["currentTime"].SetValue(Time.CurrentTime);
        }

        private void CreateParticle()
        {
            Vector2 position = portal.Position + new Vector2(NextFloat(portal.Size.X / 2f), NextFloat(-portal.Size.Y / 2f, portal.Size.Y / 2f)).Rotate(portal.Rotation);
            float angle = NextFloat(-maxSpeedAngle, maxSpeedAngle);
            Vector2 speed = (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * NextFloat(minSpeed, maxSpeed)).Rotate(portal.Rotation);
            float rotationSpeed = NextFloat(minRotationSpeed, maxRotationSpeed);

            Color particleColor = color * NextFloat(minColorMultiplier, maxColorMultiplier);

            angle = NextFloat(-(float)Math.PI, (float)Math.PI);
            vertexesData[index] = new ParticleVertexData(position, Vector2.UnitX.Rotate(angle) * NextFloat(minRadius, maxRadius), speed, rotationSpeed, Time.CurrentTime, particleColor);

            angle += NextFloat(minVertexAngle, maxVertexAngle);
            vertexesData[index + 1] = new ParticleVertexData(position, Vector2.UnitX.Rotate(angle) * NextFloat(minRadius, maxRadius), speed, rotationSpeed, Time.CurrentTime, particleColor);

            angle += NextFloat(minVertexAngle, maxVertexAngle);
            vertexesData[index + 2] = new ParticleVertexData(position, Vector2.UnitX.Rotate(angle) * NextFloat(minRadius, maxRadius), speed, rotationSpeed, Time.CurrentTime, particleColor);

            index = (index + 3) % VertexesCount;
        }

        private float NextFloat(float min, float max)
        {
            return NextFloat() * (max - min) + min;
        }
        private float NextFloat(float max)
        {
            return NextFloat() * max;
        }
        private float NextFloat()
        {
            randomIndex = (randomIndex + 1) % randomValuesCount;
            return randomValues[randomIndex];
        }

        public void Update()
        {
            timeToCreate -= Time.FixedDeltaTime;
            while (timeToCreate <= 0f)
            {
                CreateParticle();
                timeToCreate += CreatingTime;
            }
        }

        public void Draw()
        {
            effect.Techniques[0].Passes[0].Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexesData, 0, TrianglesCount);
        }
    }
}