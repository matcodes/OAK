using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Oak.Controls
{
    #region AppTextCommand
    public class AppTextCommand : AppLabel
    {
        #region Static members
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(AppTextCommand), null, BindingMode.OneWay);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(AppTextCommand), null, BindingMode.OneWay);
        #endregion

        public AppTextCommand() : base()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, args) =>
            {
                if ((this.Command != null) && (this.Command.CanExecute(this.CommandParameter)))
                {
                    this.Opacity = 0.6;
                    this.FadeTo(1);

                    this.Command.Execute(this.CommandParameter);
                }
            };

            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public ICommand Command
        {
            get { return (this.GetValue(CommandProperty) as ICommand); }
            set { this.SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }
    }
    #endregion
}
