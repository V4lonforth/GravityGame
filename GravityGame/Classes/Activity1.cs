using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using GravityGame;
using GravityGame.Utils;

namespace PuzzleGame
{
    [Activity(Label = "GravityGame"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnPause()
        {
            base.OnPause();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            var g = new Game1();

            Screen.CreateData(Resources.DisplayMetrics);

            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}