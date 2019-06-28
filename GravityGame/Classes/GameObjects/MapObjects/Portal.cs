using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects.Base;
using GravityGame.GameObjects.Base;
using GravityGame.Effects;

namespace GravityGame.GameObjects.MapObjects
{
    public class Portal : MapObject, IGameObject, ICollider
    {
        private class Frame : Drawable, ICollider
        {
            private IGameObject portal;
            private Polygon polygon;

            private float localRotation;
            private Vector2 localPosition;

            private static Vector2 spriteSize = new Vector2(31.6f, 10f);
            private static Vector2[] vertices = new Vector2[]
            {
                new Vector2(-spriteSize.X / 2f, -spriteSize.Y / 2f),
                new Vector2(spriteSize.X / 2f, -spriteSize.Y / 2f),
                new Vector2(spriteSize.X / 2f * 0.7f, spriteSize.Y / 2f),
                new Vector2(-spriteSize.X / 2f * 0.7f, spriteSize.Y / 2f)
            };
            private static Texture2D defaultSprite;

            public Frame(IGameObject portal, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) : base(defaultSprite, Vector2.Zero, spriteSize, 0f, color, spriteEffects, depth)
            {
                this.portal = portal;

                localRotation = rotation;
                localPosition = new Vector2(0f, -(portal.Size.Y + spriteSize.Y) / 2f);
                polygon = new Polygon(vertices);
            }

            public static void LoadContent(ContentManager content)
            {
                defaultSprite = content.Load<Texture2D>("GameObjects/PortalFrame");
            }

            public void Collide(Player player)
            {
                polygon.Collide(player);
            }

            public new void Update()
            {
                Rotation = localRotation + portal.Rotation;
                Position = localPosition.Rotate(Rotation) + portal.Position;
                polygon.Update(Position, Rotation);
            }
        }

        private Frame topFrame;
        private Frame bottomFrame;
        private Polygon polygon;

        private ParticlesDrawer particlesDrawer;

        public Portal NextPortal { get; set; }

        private static Texture2D defaultSprite;
        private static Texture2D mapSprite;

        private const float MapSpriteWidth = 110f;

        public Portal(IMovingTrajectory movingTrajectory, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(movingTrajectory, defaultSprite, size, rotation, color, spriteEffects, depth)
        {
            topFrame = new Frame(this, 0f);
            bottomFrame = new Frame(this, (float)Math.PI);

            polygon = new Polygon(Vector2.Zero, size, 0f);
            particlesDrawer = new ParticlesDrawer(this, Color);
            Update();
        }

        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Rectangle");
            mapSprite = content.Load<Texture2D>("GameObjects/Rectangle");
            Frame.LoadContent(content);
        }

        public void Collide(Player player)
        {
            topFrame.Collide(player);
            bottomFrame.Collide(player);

            if (player.State == PlayerState.Free)
            {
                if (polygon.CheckCollision(player, out Vector2 hit))
                {
                    if (Vector2.Dot(player.Velocity, new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation))) < 0f)
                        player.StartTeleporting(this);
                    else
                    {
                        polygon.Collide(player);
                        return;
                    }
                }
            }
            if (player.Portal != this)
                return;
            if (player.State == PlayerState.StartedTeleporting)
            {
                if (Vector2.Dot(player.Velocity, Position - player.Position) < 0f)
                    player.Teleport();
                else
                    return;
            }
            else if (player.State == PlayerState.Teleported)
            {
                player.StartEndingTeleporting();
                if (!polygon.CheckCollision(player, out Vector2 hit))
                    player.EndTeleporting();
            }
        }

        public new void Update()
        {
            base.Update();
            topFrame.Update();
            bottomFrame.Update();
            polygon.Update(Position, Rotation);
            particlesDrawer.Update();
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            particlesDrawer.Draw();
            base.Draw(spriteBatch);
            topFrame.Draw(spriteBatch);
            bottomFrame.Draw(spriteBatch);
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            Drawable drawable = new Drawable(mapSprite, new Vector2(MapSpriteWidth / 2, 0f).Rotate(Rotation + (float)Math.PI) + Position, new Vector2(MapSpriteWidth + Size.X, Size.Y), Rotation, Color.Black);
            drawable.Draw(spriteBatch);
        }
    }
}