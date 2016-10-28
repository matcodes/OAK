using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppActivityIndicator
    public class AppActivityIndicator : ActivityIndicator
    {
        #region Static members
        public static BindableProperty IndicatorColorProperty = BindableProperty.Create("IndicatorColor", typeof(AppActivityIndicatorColors), typeof(AppActivityIndicator), AppActivityIndicatorColors.Black);
        #endregion

        public AppActivityIndicator() : base()
        {
        }

        public AppActivityIndicatorColors IndicatorColor
        {
            get { return (AppActivityIndicatorColors)this.GetValue(IndicatorColorProperty); }
            set { this.SetValue(IndicatorColorProperty, value); }
        }
    }
    #endregion
}
