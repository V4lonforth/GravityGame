using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using GravityGame.Utils;
using System.Xml.Serialization;
using GravityGame.Levels;

namespace GravityGame
{
    [Activity(Label = "GravityGame"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class Activity1 : AndroidGameActivity
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

            Screen.CreateData(new Point(Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels));

            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}