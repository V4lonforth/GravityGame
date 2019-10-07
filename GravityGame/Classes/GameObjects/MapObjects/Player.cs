using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.Base;
using GravityGame.Effects;
using GravityGame.Utils;
using GravityGame.GameObjects.MapObjects.Base;

namespace GravityGame.GameObjects.MapObjects
{
    public class Player : Drawable, IGameObject
    {
        private MovingSpiral dyingTrajectory;
        private Vector2 lastPosition;

        private TrailDrawer trail;
        private Drawable mirroredDrawable;

        private Portal portal;
        private float timeOutsidePortal;

        public bool Teleported { get; private set; }
        public bool IsOutsidePortal { get => timeOutsidePortal >= TrailDrawer.TrailTime; }

        public Vector2 Velocity { get; set; }

        private static Texture2D defaultSprite;
        private static Vector2 size = new Vector2(50);

        public Player(Vector2 position, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(defaultSprite, position, size, 0f, new Color(62, 181, 241), spriteEffects, depth)
        {
            trail = new TrailDrawer(this, new Color(Color * 0.8f, 1f), (Size.X - 12f) / 2f);
            mirroredDrawable = new Drawable(defaultSprite, position, size, 0f, new Color(62, 181, 241), spriteEffects, depth);
        }

        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Circle");
            Contour.LoadContent(content);
        }

        public void SetStartPosition(Vector2 startPosition, Vector2 launchPosition, float launchForce)
        {
            Position = startPosition;
            Velocity = (startPosition - launchPosition) * launchForce;
        }

        public void Launch(Vector2 startPosition, Vector2 launchPosition, float launchForce)
        {
            SetStartPosition(startPosition, launchPosition, launchForce);
            trail.Launch();
        }

        public void SetPortal(Portal portal)
        {
            timeOutsidePortal = 0f;
            this.portal = portal;
        }

        public void Teleport()
        {
            timeOutsidePortal = 0f;
            Teleported = true;
            Velocity = Velocity.Rotate(portal.NextPortal.Rotation - portal.Rotation + (float)Math.PI);
            Vector2 newPosition = GetMirroredPosition(portal);
            trail.CreateGap(Position, newPosition);
            Position = newPosition;
        }

        private Vector2 GetMirroredPosition(Portal portal)
        {
            Vector2 localPosition = (Position - portal.Position).Rotate(-portal.Rotation);
            localPosition.X = -localPosition.X;
            return localPosition.Rotate(portal.NextPortal.Rotation) + portal.NextPortal.Position;
        }

        public void StartDying(Gravity gravity, Time time)
        {
            if (dyingTrajectory != null)
                return;
            Vector2 currentLocalPosition = Position - gravity.Position;
            float startRadius = currentLocalPosition.Length();
            float startAngle = (float)Math.Atan2(currentLocalPosition.Y, currentLocalPosition.X);

            Vector2 localPosition = lastPosition - gravity.Position;
            float radiusSpeed = -Vector2.Dot(Velocity, localPosition);
            float angleSpeed = -Vector2.Dot(Velocity, new Vector2(localPosition.Y, -localPosition.X)) / startRadius;

            dyingTrajectory = new MovingSpiral(gravity, Position, Velocity, time.CurrentTime);
            Position = dyingTrajectory.GetPosition(time.CurrentTime);
        }

        public void Update(Time time)
        {
            lastPosition = Position;
            if (dyingTrajectory == null)
            {
                Teleported = false;
                Position += Velocity * time.FixedDeltaTime;
                timeOutsidePortal += time.FixedDeltaTime;
            }
            else
            {
                Position = dyingTrajectory.GetPosition(time.CurrentTime);
                Size = size * dyingTrajectory.Size;
                trail.Width = (size.X - 12f) / 2f * dyingTrajectory.Size;
            }
        }

        public void UpdateEffects(Time time)
        {
            trail.Update(time);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            trail.Draw();

            if (portal != null)
            {
                mirroredDrawable.Position = GetMirroredPosition(portal);
                mirroredDrawable.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}