using Android.App;
using Android.Content.PM;
using Android.OS;
using static AndroidX.Core.SplashScreen.SplashScreen;

namespace WhmCalcMaui
{
    [Activity(Theme = "@style/Theme.Animated", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // сплеш с анимацией
            // https://dev.to/icebeam7/animated-splash-screen-in-net-maui-android-2ipg
            var splash = InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);
        }
    }
}
