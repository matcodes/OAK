using Oak.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Oak.Classes.Converters
{
    #region ProductCategoriesToTextConverter
    public class ProductCategoriesToTextConverter : IValueConverter
    {
        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var productCategory = (ProductCategories)value;
            var result = "";
            if (productCategory == ProductCategories.Alcohol)
                result = "ALCOHOL";
            else if (productCategory == ProductCategories.Milk)
                result = "MILK";
            else if (productCategory == ProductCategories.Oil)
                result = "OIL";
            else if (productCategory == ProductCategories.Water)
                result = "WATER";
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
