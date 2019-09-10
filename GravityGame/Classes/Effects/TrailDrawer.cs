using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;

namespace GravityGame.Effects
{
    public class TrailDrawer
    {
        private bool active;
        private Color color;

        private TrailVertexData[] vertexesData;
        private int index;

        private Player parent;

        public float Width;
        public Vector2 Position { get; set; }

        private static int[] indexes;

        private const float timeOffsetChangeSpeed = 0.5f;
        private const float startTimeOffset = 1f;

        private const int VerticesPerTrail = 3;
        private const int TrianglesPerTrail = 4;
        private const int SectionsCount = (int)(TrailsPerSecond * TrailTime);
        private const int TrianglesCount = SectionsCount * TrianglesPerTrail;
        private const int VertexesCount = SectionsCount * VerticesPerTrail;

        private const float TrailsPerSecond = 60f;
        public const float TrailTime = 0.7f;

        private static GraphicsDevice graphicsDevice;
        private static Effect effect;

        public TrailDrawer(Player parent, Color color, float width)
        {
            this.parent = parent;
            this.color = color;
            Width = width;
            vertexesData = new TrailVertexData[VertexesCount];

            //previousLeft = new VertexData(Vector2.Zero, parent.Position, new Vector2(1f, 1f), trailsController.CurrentTime - timeOffset, TrailTime);
            //previousRight = new VertexData(Vector2.Zero, parent.Position, new Vector2(1f, 0f), trailsController.CurrentTime - timeOffset, TrailTime);
        }

        static TrailDrawer()
        {
            int[] bufIndicies = new int[12]
            {
                4, 0, 1,
                4, 3, 0,
                4, 1, 2,
                4, 2, 5
            };

            indexes = new int[TrianglesCount * 3];
            for (int i = 0; i < TrianglesCount * 3; i += 12)
                for (int j = 0; j < 12; j++)
                    indexes[i + j] = (bufIndicies[j] + i / 4) % VertexesCount;
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            effect = content.Load<Effect>("Shaders/Trail");

            effect.Parameters["duration"].SetValue(TrailTime);
            effect.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));
        }

        public void Launch()
        {
            active = true;
            Position = parent.Position;
            for (int i = 0; i < VertexesCount; i++)
                vertexesData[i].EndPosition = vertexesData[i].StartPosition = Position;
        }

        private void CreateTrail(Vector2 firstPos, Vector2 secondPos, Time time)
        {
            Vector2 direction = (secondPos - firstPos).Normalized(out float length);
            Vector2 normal = Vector2.UnitY.Rotate(direction);

            vertexesData[index] = new TrailVertexData(secondPos + normal * Width, secondPos, time.CurrentTime + TrailTime);
            vertexesData[index + 1] = new TrailVertexData(secondPos, secondPos, time.CurrentTime + TrailTime);
            vertexesData[index + 2] = new TrailVertexData(secondPos - normal * Width, secondPos, time.CurrentTime + TrailTime);

            Vector2 previousLeft = vertexesData[(index - 3 + VertexesCount) % VertexesCount].StartPosition;
            Vector2 previousRight = vertexesData[(index - 1 + VertexesCount) % VertexesCount].StartPosition;

            previousLeft = (vertexesData[index].StartPosition + previousLeft) / 2f;
            previousRight = (vertexesData[index + 2].StartPosition + previousRight) / 2f;
            Vector2 diff = previousLeft - previousRight;
            float diffWidth = diff.Length();
            if (diffWidth < Width * 2f)
            {
                diff /= diffWidth;
                float newWidth = Width * 2f - diffWidth;
                previousRight -= diff * newWidth;
                previousLeft += diff * newWidth;
            }
            vertexesData[(index - 3 + VertexesCount) % VertexesCount].StartPosition = previousLeft;
            vertexesData[(index - 1 + VertexesCount) % VertexesCount].StartPosition = previousRight;

            index = (index + 3) % VertexesCount;
        }
        public void CreateGap(Vector2 start, Vector2 end)
        {
            vertexesData[index] = vertexesData[index + 1] = vertexesData[index + 2] = new TrailVertexData(start, start, 0f);
            index = (index + 3) % VertexesCount;
            vertexesData[index] = vertexesData[index + 1] = vertexesData[index + 2] = new TrailVertexData(end, end, 0f);
            index = (index + 3) % VertexesCount;
        }

        public void Update(Time time)
        {
            effect.Parameters["worldMatrix"].SetValue(Screen.SceneMatrix);
            effect.Parameters["currentTime"].SetValue(time.CurrentTime);

            CreateTrail(Position, parent.Position, time);
            Position = parent.Position;
        }

        public void Draw()
        {
            if (active)
            {
                effect.Parameters["color"].SetValue(color.ToVector4());
                effect.Techniques[0].Passes[0].Apply();
                if (index == 0)
                {
                    graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexesData, 0, VertexesCount, indexes, 0, TrianglesCount);
                }
                else
                {
                    graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexesData, 0, VertexesCount, indexes, 0, index / 3 * 4);
                    graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexesData, 0, VertexesCount, indexes, index / 3 * 12, TrianglesCount - index / 3 * 4);
                }
            }
        }
    }
}