using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GravityGame.Controllers;
using GravityGame.Utils;

namespace GravityGame.GUI
{
    public class GUIController : IController
    {
        private IGUIElement Root { get; set; }
        private Time time;

        private bool screenLocked;

        private static SpriteFont spriteFont;

        public GUIController()
        {
            Root = new GUIElement();
            time = new Time();
        }

        public static void LoadContent(ContentManager Content)
        {
            spriteFont = Content.Load<SpriteFont>("GUI/Fonts/Font");
        }

        public void LoadMainMenu()
        {
            screenLocked = true;

        }

        public bool CheckInput(TouchLocation touch)
        {
            return Root.CheckTouch(touch) || screenLocked;
        }

        public void Update()
        {
            Root.Update(time);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Screen.GUIMatrix);
            Root.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}