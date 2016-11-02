using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Oak.Classes.Converters
{
    #region TestResultToImageConverter
    public class TestResultToImageConverter : IValueConverter
    {
        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (TestResults)value;
            var result = "";
            if (val == TestResults.OK)
                result = "green_check";
            else if (val == TestResults.Warning)
                result = "warning_check";
            else
                result = "danger_check";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}
