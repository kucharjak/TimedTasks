//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;
//using Xamarin.Forms;

//namespace TimedTasks.Converters
//{
//    class ItemSelectedBoolConverter<T> : IValueConverter
//    {
//        public IList<T> ItemsList { get; set; }

//        public ItemSelectedBoolConverter()
//        {
//            ItemsList = new List<T>();
//        }

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (ItemsList == null || !(value is T) || ItemsList.Count == 0 || !ItemsList.Contains((T)value))
//                return false;

//            return true;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return false;
//        }
//    }
//}
