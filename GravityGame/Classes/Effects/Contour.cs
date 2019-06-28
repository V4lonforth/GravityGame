using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.Base;

namespace GravityGame.Effects
{
    public class Contour
    {
        private List<WrappedDrawable> trajectory;
        private int sectionsCount;

        private static Color color = Color.Black;
        private static Texture2D sprite;

        public const int TrajectorySections = 120;
        public const float TrajectoryLength = 1000f;
        private const float TrajectoryWidth = 5f;

        public Contour()
        {
            trajectory = new List<WrappedDrawable>(TrajectorySections);
        }

        public static void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("GameObjects/Trajectory");
        }

        public void CreateContour(List<List<Vector2>> trajectoryPositions)
        {
            float width = 0f;
            sectionsCount = 0;
            int maxSectionsCount = 0;
            foreach (List<Vector2> positions in trajectoryPositions)
                maxSectionsCount += positions.Count;

            foreach (List<Vector2> positions in trajectoryPositions)
            {
                for (int i = 0; i < positions.Count - 1; i++)
                {
                    if (trajectory.Count == sectionsCount)
                    {
                        trajectory.Add(new WrappedDrawable(sprite, color));
                    }
                    WrappedDrawable drawable = trajectory[sectionsCount];
                    Vector2 direction = positions[i + 1] - positions[i];
                    float dist = direction.Length();
                    direction /= dist;
                    drawable.StartWidth = width;
                    drawable.Position = (positions[i + 1] + positions[i]) / 2f;
                    drawable.Size = new Vector2(dist, TrajectoryWidth);
                    drawable.Rotation = (float)Math.Atan2(direction.Y, direction.X);
                    float alpha = 1f - (float)i / maxSectionsCount;
                    drawable.Color = color * alpha;
                    width += dist;
                    sectionsCount++;
                }
            }
        }

        public void Clear()
        {
            trajectory.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < sectionsCount; i++)
            {
                trajectory[i].Draw(spriteBatch);
            }
        }
    }
}