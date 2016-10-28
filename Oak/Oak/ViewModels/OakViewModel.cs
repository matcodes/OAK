using Oak.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Oak.ViewModels
{
    #region OakViewModel
    public class OakViewModel : BaseViewModel
    {
        public OakViewModel() : base("", "")
        {
        }
        protected override void DoPropertyChanged(string propertyName)
        {
            if (propertyName == "IsBusy")
                this.SetCommandsState();

            base.DoPropertyChanged(propertyName);
        }

        private void SetCommandsState()
        {
            Device.BeginInvokeOnMainThread(() => {
                if (this.IsBusy)
                    this.DisableCommands();
                else
                    this.EnabledCommands();
            });
        }

        protected virtual void DisableCommands()
        {
        }

        protected virtual void EnabledCommands()
        {
        }
    }
    #endregion
}
