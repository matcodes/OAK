using Oak.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Views
{
    #region OakPage
    public class OakPage : ContentPage
    {
        public OakPage() : base()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            this.SizeChanged += (sender, args) => {
                if (this.Content == null)
                {

                    bool added = false;
                    int count = 0;

                    while ((!added) && (count < 3))
                    {
                        try
                        {
                            var content = this.BuildPageContent();
                            if (content != null)
                                this.Content = content;
                            added = true;
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("BuildPageContent exception: {0}", exception.Message);
                            count++;
                            //if (count == 3)
                            //    ShowToastMessage.Send("Error create view. " + exception.Message);
                        }
                    }
                }
            };
        }

        private View BuildPageContent()
        {
            var busyIndicator = new ActivityIndicator
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            busyIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy", BindingMode.TwoWay);

            var pageContent = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0
            };
            pageContent.Children.Add(this.CreateContent());
            pageContent.Children.Add(busyIndicator);

            return pageContent;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.ViewModel != null)
                this.ViewModel.Appering();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (this.ViewModel != null)
                this.ViewModel.Disappering();
        }

        protected override bool OnBackButtonPressed()
        {
            var result = base.OnBackButtonPressed();
            if (this.ViewModel != null)
                result = this.ViewModel.HandleBackButton();
            return result;
        }

        protected virtual View CreateContent()
        {
            return null;
        }

        public OakViewModel ViewModel { get; protected set; }
    }
    #endregion
}
