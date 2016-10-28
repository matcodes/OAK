using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.Views
{
    #region MainPage
    public class MainPage : NavigationPage
    {
        #region Static members
        private static Page CreateStartPage()
        {
            var page = new StartPage();
            page.ViewModel.Initialize();
            return page;
        }
        #endregion

        public MainPage() : base(CreateStartPage())
        {
        }
    }
    #endregion
}
