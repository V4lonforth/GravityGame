using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GravityGame.GameObjects.MapObjects.Base;
using GravityGame.GameObjects.Base;
using GravityGame.Utils;

namespace GravityGame.GameObjects.MapObjects
{
    public class Star : MapObject, IGameObject
    {
        private List<IGameObject> players;

        private static Texture2D defaultSprite;

        public Star(IMovingTrajectory movingTrajectory, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 0.5F) 
            : base(movingTrajectory, defaultSprite, size, rotation, color, spriteEffects, depth)
        {
            players = new List<IGameObject>();
        }

        public static void LoadContent(ContentManager content)
        {
            defaultSprite = content.Load<Texture2D>("GameObjects/Star");
        }

        public bool TryGainStar(IGameObject player)
        {
            if (players.Contains(player) || !CheckCollision(player))
                return false;
            players.Add(player);
            return true;
        }

        public bool IsStarGained(IGameObject player)
        {
            return players.Contains(player);
        }

        public bool RemovePlayer(IGameObject player)
        {
            return players.Remove(player);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (players.Count == 0)
                base.Draw(spriteBatch);
        }

        public void UpdateEffects(Time time)
        {
        }
    }
}