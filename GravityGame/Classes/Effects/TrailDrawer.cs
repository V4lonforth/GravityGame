using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GravityGame.GameObjects.MapObjects;
using GravityGame.Utils;

namespace GravityGame.Effects
{
    public class TrailDrawer
    {
        private Color color;

        private TrailVertexData[] vertexesData;
        private int index;

        private Player parent;
        private float width;

        public Vector2 Position { get; set; }

        private static int[] indices;

        private const float timeOffsetChangeSpeed = 0.5f;
        private const float startTimeOffset = 1f;

        private const int VerticesPerTrail = 3;
        private const int TrianglesPerTrail = 4;
        private const int SectionsCount = (int)(TrailsPerSecond * TrailTime);
        private const int TrianglesCount = (SectionsCount + 1) * TrianglesPerTrail;
        private const int VertexesCount = SectionsCount * VerticesPerTrail;

        private const float TrailsPerSecond = 60f;
        public const float TrailTime = 0.7f;

        private static GraphicsDevice graphicsDevice;
        private static Effect effect;

        public TrailDrawer(Player parent, Color color, float width)
        {
            this.parent = parent;
            this.color = color;
            this.width = width;
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

            indices = new int[TrianglesCount * 3];
            for (int i = 0; i < TrianglesCount * 3; i += 12)
                for (int j = 0; j < 12; j++)
                    indices[i + j] = (bufIndicies[j] + i / 4) % VertexesCount;
        }

        public static void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            effect = content.Load<Effect>("Shaders/Trail");

            effect.Parameters["duration"].SetValue(TrailTime);
            effect.Parameters["width"].SetValue((float)(Screen.ScreenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(Screen.ScreenSize.Y / 2));
        }
        public static void UpdateEffect()
        {
            effect.Parameters["worldMatrix"].SetValue(Screen.SceneMatrix);
            effect.Parameters["currentTime"].SetValue(Time.CurrentTime);
        }

        public void Launch()
        {
            Position = parent.Position;
            for (int i = 0; i < VertexesCount; i++)
                vertexesData[i].EndPosition = vertexesData[i].StartPosition = Position;
        }

        private void CreateTrail(Vector2 firstPos, Vector2 secondPos)
        {
            Vector2 direction = (secondPos - firstPos).Normalized(out float length);
            Vector2 normal = Vector2.UnitY.Rotate(direction);

            vertexesData[index] = new TrailVertexData(secondPos + normal * width, secondPos, Time.CurrentTime + TrailTime);
            vertexesData[index + 1] = new TrailVertexData(secondPos, secondPos, Time.CurrentTime + TrailTime);
            vertexesData[index + 2] = new TrailVertexData(secondPos - normal * width, secondPos, Time.CurrentTime + TrailTime);

            Vector2 previousLeft = vertexesData[(index - 3 + VertexesCount) % VertexesCount].StartPosition;
            Vector2 previousRight = vertexesData[(index - 1 + VertexesCount) % VertexesCount].StartPosition;

            previousLeft = (vertexesData[index].StartPosition + previousLeft) / 2f;
            previousRight = (vertexesData[index + 2].StartPosition + previousRight) / 2f;
            Vector2 diff = previousLeft - previousRight;
            float diffWidth = diff.Length();
            if (diffWidth < width * 2f)
            {
                diff /= diffWidth;
                float newWidth = width * 2f - diffWidth;
                previousRight -= diff * newWidth;
                previousLeft += diff * newWidth;
            }
            vertexesData[(index - 3 + VertexesCount) % VertexesCount].StartPosition = previousLeft;
            vertexesData[(index - 1 + VertexesCount) % VertexesCount].StartPosition = previousRight;

            index = (index + 3) % VertexesCount;
        }
        public void CreateEmptyTrail(Vector2 start, Vector2 end)
        {
            vertexesData[index] = vertexesData[index + 1] = vertexesData[index + 2] = new TrailVertexData(start, start, 0f);
            index = (index + 3) % VertexesCount;
            vertexesData[index] = vertexesData[index + 1] = vertexesData[index + 2] = new TrailVertexData(end, end, 0f);
            index = (index + 3) % VertexesCount;
        }
        public void Update()
        {
            CreateTrail(Position, parent.Position);
            Position = parent.Position;
        }
        public void Draw()
        {
            effect.Parameters["color"].SetValue(color.ToVector4());
            effect.Techniques[0].Passes[0].Apply();
            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexesData, 0, VertexesCount, indices, 0, TrianglesCount - 4);
        }
    }
}