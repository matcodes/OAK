using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Oak.Classes
{
    #region ExecuteVisualCommandMethod
    public delegate void ExecuteVisualCommandMethod(object parameter);
    #endregion

    #region VisualCommand
    public class VisualCommand : ICommand
    {
        public VisualCommand(ExecuteVisualCommandMethod executeMethod)
        {
            this.ExecuteMethod = executeMethod;
        }

        private void DoCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, EventArgs.Empty);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    this.DoCanExecuteChanged();
                }
            }
        }

        private ExecuteVisualCommandMethod ExecuteMethod { get; set; }

        #region ICommand implementation
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.IsEnabled;
        }

        public void Execute(object parameter)
        {
            if ((this.CanExecute(parameter)) && (this.ExecuteMethod != null))
                this.ExecuteMethod(parameter);
        }
        #endregion
    }
    #endregion
}
