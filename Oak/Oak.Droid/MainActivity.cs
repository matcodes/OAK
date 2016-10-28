using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Oak.Classes.Messages;

namespace Oak.Droid
{
	[Activity (Label = "OAK", 
        Icon = "@drawable/icon", 
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Display display = this.WindowManager.DefaultDisplay;
            var size = new Point();
            display.GetSize(size);

            global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new Oak.App ());
		}

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();

            CloseAppMessage.Subscribe(this, this.CloseApp);
        }

        protected override void OnPause()
        {
            base.OnPause();

            CloseAppMessage.Unsubscribe(this);
        }

        private void CloseApp(CloseAppMessage message)
        {
            this.FinishAffinity();
        }
	}
}

