using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;

namespace GravityGame.Effects.PortalEffects
{
    public class PortalParticlesDrawer : ParticlesDrawer<PortalParticleVertexData>
    {
        private Portal portal;
        private Color color;

        private static Effect effect;

        private const float MinRadius = 2f;
        private const float MaxRadius = 5f;

        private const float MinRotationSpeed = (float)Math.PI / 2f;
        private const float MaxRotationSpeed = (float)Math.PI;

        private const float MinSpeed = 4f;
        private const float MaxSpeed = 21f;
        private const float MaxSpeedAngle = 0.3f;

        private const float MinVertexAngle = (float)Math.PI / 2f;
        private const float MaxVertexAngle = (float)Math.PI * 5f / 6f;

        private const float CreationTime = 0.022f;
        private const float LifeTime = 1.8f;

        private const float MinColorMultiplier = 0.5f;
        private const float MaxColorMultiplier = 1.5f;

        private const int TrianglesCount = (int)(LifeTime / CreationTime + 1f);
        private const int VertexesCount = TrianglesCount * 3;

        public PortalParticlesDrawer(Portal portal, Color color) : base(MinRadius, MaxRadius, MinVertexAngle, MaxVertexAngle, CreationTime, TrianglesCount, effect)
        {
            this.portal = portal;
            this.color = color;
        }

        public static void LoadContent(ContentManager content)
        {
            effect = content.Load<Effect>("Shaders/PortalParticles");
            effect.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));
            effect.Parameters["duration"].SetValue(LifeTime);
        }

        public new void Update(Time time)
        {
            base.Update(time);
            effect.Parameters["worldMatrix"].SetValue(Screen.SceneMatrix);
            effect.Parameters["currentTime"].SetValue(time.CurrentTime);
        }

        protected override void CreateParticle(Time time)
        {
            Vector2 position = portal.Position + new Vector2(floatRandom.NextFloat(portal.Size.X / 2f), floatRandom.NextFloat(-portal.Size.Y / 2f, portal.Size.Y / 2f)).Rotate(portal.Rotation);
            float angle = floatRandom.NextFloat(-MaxSpeedAngle, MaxSpeedAngle);
            Vector2 speed = (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * floatRandom.NextFloat(MinSpeed, MaxSpeed)).Rotate(portal.Rotation);
            float rotationSpeed = floatRandom.NextFloat(MinRotationSpeed, MaxRotationSpeed);

            Color particleColor = color * floatRandom.NextFloat(MinColorMultiplier, MaxColorMultiplier);

            Vector2[] vertexes = CreateTriangle();
            AddVertexesData(new PortalParticleVertexData[]
            {
                new PortalParticleVertexData(position, vertexes[0], speed, rotationSpeed, time.CurrentTime, particleColor),
                new PortalParticleVertexData(position, vertexes[1], speed, rotationSpeed, time.CurrentTime, particleColor),
                new PortalParticleVertexData(position, vertexes[2], speed, rotationSpeed, time.CurrentTime, particleColor)
            }); 
        }
    }
}