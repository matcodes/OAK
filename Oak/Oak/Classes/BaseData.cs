using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Oak.Classes
{
    #region BaseData
    public class BaseData : INotifyPropertyChanged, IDisposable
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        private object _setterLock = new object();

        public BaseData()
        {
        }

        protected virtual void DoPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected object GetValue(string propertyName)
        {
            object result = null;
            _values.TryGetValue(propertyName, out result);
            return result;
        }

        protected object GetValue(string propertyName, object defaultValue)
        {
            object result = this.GetValue(propertyName);
            if (result == null)
                result = defaultValue;
            return result;
        }

        protected void SetValue(string propertyName, object value)
        {
            var oldValue = this.GetValue(propertyName);
            if ((value != oldValue) || (oldValue == null))
            {
                lock (_setterLock)
                {
                    if (_values.ContainsKey(propertyName))
                        _values[propertyName] = value;
                    else
                        _values.Add(propertyName, value);
                }

                this.DoPropertyChanged(propertyName);
            }
        }

        #region IDisposable implementation
        public virtual void Dispose()
        {
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    #endregion
}
