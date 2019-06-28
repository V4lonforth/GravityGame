using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GravityGame.GUI.Animation;

namespace GravityGame.GUI
{
    public class Button : GUIElement, IGUIElement
    {
        private readonly Func<Button, bool> Action;

        private bool pressed;
        private int touchId;

        public Button(Func<Button, bool> action, Texture2D sprite, Vector2 position, Vector2 size, float rotation, Color? color = null, SpriteEffects spriteEffects = SpriteEffects.None, float depth = 1, GUIElement parent = null, IGUIAnimation animation = null) : base(sprite, position, size, rotation, color, spriteEffects, depth, parent, animation)
        {
            Action = action;
        }

        public new bool CheckTouch(TouchLocation touch)
        {
            if (base.CheckTouch(touch))
                return true;

            if (!pressed && touch.State == TouchLocationState.Pressed)
            {
                Vector2 pos = touch.Position.ScreenToGUIPosition();
                if (!pressed && InBounds(pos))
                {
                    touchId = touch.Id;
                    Press();
                    return true;
                }
                else
                    return false;
            }
            if (!pressed || touchId != touch.Id)
                return false;

            if (touch.State == TouchLocationState.Moved)
            {
                Hold();
            }
            else if (touch.State == TouchLocationState.Released)
            {
                Release(touch);
            }

            return true;
        }

        private bool InBounds(Vector2 pos)
        {
            if (pos.X < Left || pos.X > Right || pos.Y < Top || pos.Y > Bottom)
                return false;
            return true;
        }

        private void Press()
        {
            pressed = true;
        }
        private void Hold()
        {
            //if (colorValue > fadedValue)
            //{
            //    colorValue -= fadeSpeed * Time.FixedDeltaTime;
            //}
            //else
            //    colorValue = 0.5f;
            //drawableObject.Color = new Color(colorValue, colorValue, colorValue);
        }
        private void Release(TouchLocation touch)
        {
            pressed = false;
            //drawableObject.Color = Color.White;
            //colorValue = 1f;
            if (InBounds(touch.Position.ScreenToGUIPosition()))
                Action(this);
        }
    }
}