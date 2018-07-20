using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TimedTasks.Converters
{
    class ObjectEqualsObjectConverter : IValueConverter
    {
        public object Comparison { get; set; } = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || Comparison == null)
                return false;

            return value.Equals(Comparison);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || Comparison == null)
                return false;

            if (value.Equals(Comparison))
                return true;

            return false;
        }
    }
}
