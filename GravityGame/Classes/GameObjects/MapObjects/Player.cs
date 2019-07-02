using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.Base;
using GravityGame.Effects;
using GravityGame.Utils;
using GravityGame.Levels;
using System;

namespace GravityGame.GameObjects.MapObjects
{
    public class Player : Drawable, IGameObject
    {
        private TrailDrawer trail;
        private bool launching;

        private Drawable mirroredDrawable;

        public PlayerState State { get; private set; }
        public Portal Portal { get; private set; }
        public Contour Contour;

        public Vector2 Velocity { get; set; }

        private static Texture2D defaultSprite;
        private static Vector2 size = new Vector2(50);

        private const float launchKoeff = 2f;

        public Player(Vector2 position, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(defaultSprite, position, size, 0f, new Color(62, 181, 241), spriteEffects, depth)
        {
            Contour = new Contour();
            trail = new TrailDrawer(this, new Color(Color * 0.8f, 1f), Radius - 6);

            mirroredDrawable = new Drawable(defaultSprite, position, size, 0f, new Color(62, 181, 241), spriteEffects, depth);
        }
        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Circle");
            Contour.LoadContent(content);
        }

        public void SetStartingPosition(Vector2 position, Vector2 startPosition, Level level)
        {
            Position = position;
            Contour.CreateContour(GetTrajectory(startPosition, level));
            launching = true;
        }

        private List<List<Vector2>> GetTrajectory(Vector2 startPosition, Level level)
        {
            Vector2 position = Position;

            Launch(startPosition - Position);
            launching = true;
            float distance = 0f;
            int sectionNumber = 0;
            Vector2 lastPosition = Position;
            List<List<Vector2>> trajectory = new List<List<Vector2>>() { new List<Vector2>() { Position } };
            int trajectoryNumber = 0;
            while (distance < Contour.TrajectoryLength && sectionNumber < Contour.TrajectorySections)
            {
                level.UpdatePlayer(this);
                Update();
                if (State != PlayerState.Teleported)
                {
                    distance += (Position - lastPosition).Length();
                    trajectory[trajectoryNumber].Add(Position);
                }
                else
                {
                    trajectoryNumber++;
                    trajectory.Add(new List<Vector2>() { Position });
                }
                lastPosition = Position;
                sectionNumber++;
            }
            Position = position;

            trajectory[0][0] = (trajectory[0][1] - trajectory[0][0]).Normalized(out float length) * Radius + trajectory[0][0];
            return trajectory;
        }

        public void Launch(Vector2 velocity)
        {
            launching = false;
            Velocity = velocity * launchKoeff;
            trail.Launch();
            State = PlayerState.Free;
        }

        public void StartTeleporting(Portal portal)
        {
            State = PlayerState.StartedTeleporting;
            Portal = portal;

        }
        public void Teleport()
        {
            Velocity = Velocity.Rotate(Portal.NextPortal.Rotation - Portal.Rotation + (float)Math.PI);
            Vector2 newPosition = GetMirroredPosition(Portal);
            trail.CreateEmptyTrail(Position, newPosition);
            Position = newPosition;
            State = PlayerState.Teleported;
        }
        public void StartEndingTeleporting()
        {
            State = PlayerState.EndingTeleporting;
        }
        public void EndTeleporting()
        {
            State = PlayerState.Free;
            Portal = null;
        }

        private Vector2 GetMirroredPosition(Portal portal)
        {
            Vector2 localPosition = (Position - portal.Position).Rotate(-portal.Rotation);
            localPosition.X = -localPosition.X;
            return localPosition.Rotate(portal.NextPortal.Rotation) + portal.NextPortal.Position;
        }

        public new void Update()
        {
            Position += Velocity * Time.FixedDeltaTime;

            if (!launching)
                trail.Update();
            base.Update();
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (launching)
                Contour.Draw(spriteBatch);
            else
            {
                trail.Draw();
                if (State != PlayerState.Free)
                {
                    if (State != PlayerState.StartedTeleporting)
                        mirroredDrawable.Position = GetMirroredPosition(Portal.NextPortal);
                    else
                        mirroredDrawable.Position = GetMirroredPosition(Portal);
                    mirroredDrawable.Draw(spriteBatch);
                }
            }

            base.Draw(spriteBatch);
        }
    }
}