using System;
using System.Globalization;
using Xamarin.Forms;

namespace TimedTasks.Converters
{
    class DateTimeToStringConverter : IValueConverter
    {
        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString(Format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DateTime();
        }
    }
}
