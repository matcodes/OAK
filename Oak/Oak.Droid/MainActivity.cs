using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Oak.Classes.Messages;
using Java.IO;
using Android.Content;
using Android.Net;

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

            this.SubscribeMessages();
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.UnsubscribeMessages();
        }

        private void SubscribeMessages()
        {
            ShowToastMessage.Subscribe(this, this.ShowToast);
            CloseAppMessage.Subscribe(this, this.CloseApp);
            SendFileByEmailMessage.Subscribe(this, this.SendFileByEmail);
        }

        private void UnsubscribeMessages()
        {
            ShowToastMessage.Unsubscribe(this);
            CloseAppMessage.Unsubscribe(this);
            SendFileByEmailMessage.Unsubscribe(this);
        }

        private void ShowToast(ShowToastMessage message)
        {
            this.RunOnUiThread(() => {
                Toast.MakeText(this, message.Text, ToastLength.Long).Show();
            });
        }

        private void CloseApp(CloseAppMessage message)
        {
            this.FinishAffinity();
        }

        private void SendFileByEmail(SendFileByEmailMessage message)
        {
            File file = new File(message.FileName);
            var path = Uri.FromFile(file);
            Intent emailIntent = new Intent(Intent.ActionSend);
            emailIntent.SetType("vnd.android.cursor.dir/email");
            emailIntent.PutExtra(Intent.ExtraStream, path);
            emailIntent.PutExtra(Intent.ExtraSubject, "OAK Scan Result");
            this.StartActivity(Intent.CreateChooser(emailIntent, "Send email..."));
        }
	}
}

