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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Content.Res;
using Oak.Controls;
using Oak.Droid.Renderers;
using Oak.Classes.Enums;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(AppLabel), typeof(AppLabelRenderer))]
namespace Oak.Droid.Renderers
{
    #region AppLabelRenderer
    public class AppLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> args)
        {
            base.OnElementChanged(args);

            this.SetAppFont();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == AppLabel.AppFontProperty.PropertyName)
                this.SetAppFont();
        }

        private void SetAppFont()
        {
            var element = (this.Element as AppLabel);

            string path = "Fonts/Montserrat-Light.otf";
            if (element.AppFont == AppFonts.MontserratBold)
                path = "Fonts/Montserrat-Bold.otf";
            else if (element.AppFont == AppFonts.MontserratRegular)
                path = "Fonts/Montserrat-Regular.otf";

            Typeface typeFace = Typeface.CreateFromAsset(this.Context.Assets, path);
            this.Control.Typeface = typeFace;
        }
    }
    #endregion
}