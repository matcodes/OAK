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
using Oak.Controls;
using Oak.Droid.Renderers;
using Oak.Classes.Enums;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(AppActivityIndicator), typeof(AppActivityIndicatorRenderer))]
namespace Oak.Droid.Renderers
{
    #region AppActivityIndicatorRenderer
    public class AppActivityIndicatorRenderer : ActivityIndicatorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> args)
        {
            base.OnElementChanged(args);

            this.SetIndicatorColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == AppActivityIndicator.IndicatorColorProperty.PropertyName)
                this.SetIndicatorColor();
        }

        private void SetIndicatorColor()
        {
            var element = (this.Element as AppActivityIndicator);

            var drawable = Context.Resources.GetDrawable("progress_black_data");
            if (element.IndicatorColor == AppActivityIndicatorColors.Green)
                drawable = Context.Resources.GetDrawable("progress_green_data");

            this.Control.IndeterminateDrawable = drawable;
        }
    }
    #endregion
}