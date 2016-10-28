using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppLabel
    public class AppLabel : Label
    {
        #region Static members
        public static BindableProperty AppFontProperty = BindableProperty.Create("AppFont", typeof(AppFonts), typeof(AppLabel), AppFonts.MontserratLight);
        #endregion

        public AppLabel() : base()
        {
        }

        public AppFonts AppFont
        {
            get { return (AppFonts)this.GetValue(AppFontProperty); }
            set { this.SetValue(AppFontProperty, value); }
        }
    }
    #endregion
}
