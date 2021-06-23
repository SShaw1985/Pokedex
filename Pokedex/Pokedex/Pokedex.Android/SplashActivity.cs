using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
namespace Pokedex.Droid
{
    [Activity(
              Icon = "@drawable/pokeball",
              Theme = "@style/SplashTheme",
              MainLauncher = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}