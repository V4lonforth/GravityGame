using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GravityGame.GameObjects.Base;
using GravityGame.GUI.Animation;
using GravityGame.Utils;

namespace GravityGame.GUI
{
    public class GUIElement : Drawable, IGUIElement
    {
        private IGUIElement parent;
        private List<IGUIElement> childs;
        private IGUIAnimation animation;

        private Vector2 localPosition;
        private float localRotation;

        public int Id { get; private set; }

        public Vector2 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                Position = parent == null ? localPosition : localPosition + parent.Position;
            }
        }
        public float LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value;
                Rotation = parent == null ? localRotation : localRotation + parent.Rotation;
            }
        }

        public float Left { get => Position.X - Size.X / 2f; }
        public float Right { get => Position.X + Size.X / 2f; }
        public float Top { get => Position.Y - Size.Y / 2f; }
        public float Bottom { get => Position.Y + Size.Y / 2f; }

        private static int lastId = 0;

        public GUIElement() : base(null)
        {
            childs = new List<IGUIElement>();
        }

        public GUIElement(Texture2D sprite, Vector2 position, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 1F, GUIElement parent = null, IGUIAnimation animation = null)
            : base(sprite, position, size, rotation, color, spriteEffects, depth)
        {
            this.parent = parent;
            this.animation = animation;

            LocalPosition = position;
            LocalRotation = rotation;

            Id = lastId++;

            childs = new List<IGUIElement>();
            if (parent != null)
                parent.childs.Add(this);
        }

        public bool CheckTouch(TouchLocation touch)
        {
            foreach (IGUIElement element in childs)
            {
                if (element.CheckTouch(touch))
                    return true;
            }
            return false;
        }

        public void Update(Time time)
        {
            if (animation != null)
                animation.Update();

            foreach (IGUIElement element in childs)
                element.Update(time);
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (IGUIElement element in childs)
                element.Draw(spriteBatch);
        }

        public void UpdateEffects(Time time)
        {
        }
    }
}