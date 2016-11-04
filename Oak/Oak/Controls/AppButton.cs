using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppButton
    public class AppButton : Button
    {
        #region Static members
        public static BindableProperty AppFontProperty = BindableProperty.Create("AppFont", typeof(AppFonts), typeof(AppButton), AppFonts.MontserratLight);
        public static BindableProperty IsEmptyProperty = BindableProperty.Create("IsEmpty", typeof(bool), typeof(AppButton), false);
        #endregion

        public AppButton() : base()
        {
            this.Clicked += async (sender, args) => {
                if ((this.Command != null) && (this.Command.CanExecute(this.CommandParameter)))
                {
                    await this.FadeTo(0.6);
                    await this.FadeTo(1);
                }
            };
        }

        public AppFonts AppFont
        {
            get { return (AppFonts)this.GetValue(AppFontProperty); }
            set { this.SetValue(AppFontProperty, value); }
        }

        public bool IsEmpty
        {
            get { return (bool)this.GetValue(IsEmptyProperty); }
            set { this.SetValue(IsEmptyProperty, value); }
        }
    }
    #endregion
}
