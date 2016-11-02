using Oak.Classes.Converters;
using Oak.Classes.Enums;
using Oak.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Oak.Views.Popups
{
    #region MessagePopup
    public class TestResultPopup : ContentView
    {
        #region Static members
        public static readonly BindableProperty ContinueCommandProperty = BindableProperty.Create("ContinueCommand", typeof(ICommand), typeof(TestResultPopup), null);
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(TestResultPopup), default(string));
        public static readonly BindableProperty ResultProperty = BindableProperty.Create("State", typeof(TestResults), typeof(TestResultPopup), TestResults.OK);
        #endregion

        public TestResultPopup() : base()
        {
            this.HorizontalOptions = LayoutOptions.Fill;
            this.VerticalOptions = LayoutOptions.Fill;

            this.Padding = new Thickness(40, 40, 40, 40);

            this.BackgroundColor = BackgroundColor = Color.FromRgba(0, 0, 0, 120);

            this.Content = this.CreateContent();
        }

        private View CreateContent()
        {
            #region Content
            var converter = new TestResultToImageConverter();

            var image = new Image {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 100,
                WidthRequest = 100,
            };
            image.SetBinding(Image.SourceProperty, new Binding("Result", BindingMode.OneWay, converter, null, null, this));

            var messageLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
            };
            messageLabel.SetBinding(Label.TextProperty, new Binding("Text", BindingMode.OneWay, null, null, null, this));

            var content = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                ColumnSpacing = 10,
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };
            content.Children.Add(image, 0, 0);
            content.Children.Add(messageLabel, 1, 0);
            #endregion

            #region Commands
            var continueCommand = new AppButton
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Text = "CONTINUE"
            };
            continueCommand.SetBinding(AppButton.CommandProperty, new Binding("ContinueCommand", BindingMode.OneWay, null, null, null, this));

            var commands = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                RowSpacing = 0,
                ColumnSpacing = 0
            };
            commands.Children.Add(continueCommand);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                RowSpacing = 10,
                ColumnSpacing = 0,
                BackgroundColor = Color.Gray,
                Padding = new Thickness(20, 10, 20, 10),
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            grid.Children.Add(content, 0, 1);
            grid.Children.Add(commands, 0, 2);

            return grid;
        }

        public ICommand ContinueCommand
        {
            get { return (ICommand)this.GetValue(ContinueCommandProperty); }
            set { this.SetValue(ContinueCommandProperty, value); }
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public TestResults Result
        {
            get { return (TestResults)this.GetValue(ResultProperty); }
            set { this.SetValue(ResultProperty, value); }
        }
    }
    #endregion
}
