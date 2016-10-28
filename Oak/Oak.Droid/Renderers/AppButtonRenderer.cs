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
using Xamarin.Forms.Platform.Android;
using Oak.Controls;
using Oak.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Oak.Classes.Enums;
using Android.Graphics;

[assembly: ExportRenderer(typeof(AppButton), typeof(AppButtonRenderer))]
namespace Oak.Droid.Renderers
{
    #region AppButtonRenderer
    public class AppButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> args)
        {
            base.OnElementChanged(args);

            this.SetAppFont();
            this.SetBackgroundResource();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == AppLabel.AppFontProperty.PropertyName)
                this.SetAppFont();
        }

        private void SetAppFont()
        {
            var element = (this.Element as AppButton);

            string path = "Fonts/Montserrat-Light.otf";
            if (element.AppFont == AppFonts.MontserratBold)
                path = "Fonts/Montserrat-Bold.otf";
            else if (element.AppFont == AppFonts.MontserratRegular)
                path = "Fonts/Montserrat-Regular.otf";

            Typeface typeFace = Typeface.CreateFromAsset(this.Context.Assets, path);
            this.Control.Typeface = typeFace;
        }

        private void SetBackgroundResource()
        {
            var element = (this.Element as AppButton);

            this.Control.SetBackgroundResource(Resource.Drawable.appbutton);
        }
    }
    #endregion
}