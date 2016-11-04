using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Oak.Controls;
using Oak.Droid.Renderers;

[assembly: ExportRenderer(typeof(AppProgressBar), typeof(AppProgressBarRenderer))]
namespace Oak.Droid.Renderers
{
    #region AppProgressBarRenderer
    public class AppProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> args)
        {
            base.OnElementChanged(args);

            this.SetColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == "Color")
                this.SetColor();
        }

        private void SetColor()
        {
            var appProgressBar = (this.Element as AppProgressBar);

            //this.Control.ProgressDrawable.SetColorFilter(appProgressBar.Color.ToAndroid(), Android.Graphics.PorterDuff.Mode.Multiply);
        }
    }
    #endregion
}