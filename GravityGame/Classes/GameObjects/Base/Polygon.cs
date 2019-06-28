using GravityGame.GameObjects.MapObjects;
using Microsoft.Xna.Framework;

namespace GravityGame.GameObjects.Base
{
    public class Polygon : ICollider
    {
        private Vector2[] baseVertices;
        private Vector2[] baseNormals;

        private Vector2[] vertices;
        private Vector2[] normals;

        private Polygon(int verticesCount)
        {
            vertices = new Vector2[verticesCount];
            normals = new Vector2[verticesCount];
        }
        public Polygon(Vector2[] vertices) : this(vertices.Length)
        {
            baseVertices = vertices;
            CreateNormals();
        }
        public Polygon(Vector2 position, Vector2 size, float rotation) : this(4)
        {
            baseVertices = new Vector2[4];
            baseVertices[0] = new Vector2(-size.X / 2f, -size.Y / 2f);
            baseVertices[1] = new Vector2(size.X / 2f, -size.Y / 2f);
            baseVertices[2] = new Vector2(size.X / 2f, size.Y / 2f);
            baseVertices[3] = new Vector2(-size.X / 2f, size.Y / 2f);

            for (int i = 0; i < 4; i++)
                baseVertices[i] = baseVertices[i].Rotate(rotation) + position;
            CreateNormals();
        }

        private void CreateNormals()
        {
            baseNormals = new Vector2[baseVertices.Length];

            for (int i = 0; i < baseNormals.Length; i++)
            {
                baseNormals[i].X = baseVertices[(i + 1) % baseVertices.Length].Y - baseVertices[i].Y;
                baseNormals[i].Y = baseVertices[i].X - baseVertices[(i + 1) % baseVertices.Length].X;
                baseNormals[i].Normalize();
            }
        }

        public void Update(Vector2 position, float rotation)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = baseVertices[i].Rotate(rotation) + position;
                normals[i] = baseNormals[i].Rotate(rotation);
            }
        }

        public void Collide(Player player)
        {
            if (CheckCollision(player, out Vector2 hit))
            {
                hit -= player.Position;
                hit.Normalize();
                float dot = Vector2.Dot(hit, player.Velocity);
                if (dot > 0f)
                    player.Velocity -= 2f * dot * hit;
            }
        }

        public bool CheckCollision(IGameObject gameObject, out Vector2 hitPos)
        {
            float minDist = float.MaxValue;
            hitPos = Vector2.Zero;
            bool found = false;
            for (int i = 0; i < vertices.Length; i++)
            {
                float dist = GetDistance(gameObject.Position, i, out Vector2 hit);
                if (dist <= gameObject.Radius && dist < minDist)
                {
                    minDist = dist;
                    hitPos = (hit - gameObject.Position) / dist * gameObject.Radius + gameObject.Position;
                    found = true;
                }
            }
            return found;
        }

        private float GetDistance(Vector2 point, int ind, out Vector2 hitPos)
        {
            if (Vector2.Dot(vertices[ind] - point, vertices[ind] - vertices[(ind + 1) % vertices.Length]) <= 0f)
            {
                hitPos = vertices[ind];
                return (point - vertices[ind]).Length();
            }
            if (Vector2.Dot(vertices[(ind + 1) % vertices.Length] - point, vertices[(ind + 1) % vertices.Length] - vertices[ind]) <= 0f)
            {
                hitPos = vertices[(ind + 1) % vertices.Length];
                return (point - vertices[(ind + 1) % vertices.Length]).Length();
            }
            float dist = Vector2.Dot(point - vertices[ind], normals[ind]);
            if (dist < 0f)
            {
                dist = float.MaxValue;
            }
            hitPos = -normals[ind] * dist + point;
            return dist;
        }
    }
}