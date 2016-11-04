using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppProgressBar
    public class AppProgressBar : ProgressBar
    {
        #region Static members
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(AppProgressBar), Color.White);
        #endregion

        public AppProgressBar() : base()
        {
        }

        public Color Color
        {
            get { return (Color)this.GetValue(ColorProperty); }
            set { this.SetValue(ColorProperty, value); }
        }
    }
    #endregion
}
