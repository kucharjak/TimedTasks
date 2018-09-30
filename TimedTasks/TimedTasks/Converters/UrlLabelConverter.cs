//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;
//using System.Text.RegularExpressions;
//using Xamarin.Forms;

//namespace TimedTasks.Converters
//{
//    public class UrlLabelConverter : IValueConverter
//    {
//        private readonly Color linkColor = Color.FromRgb(80, 156, 197);

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if ((value as string) == null)
//                return value;

//            value =
//@"asd http://www.google.cz/search?=xamarin+adding+hypertext+to+label&rlz=1C1ASUM_enCZ701CZ701&oq=xamarin+adding+hypertext+to+label&aqs=chrome..69i57.12487j0j7&sourceid=chrome&ie=UTF-8 asdsad sad ¨
//asda

//sasddasdad http://www.google.cz/";

//            var regex = new Regex(@"(https?|ftp)://[^\s/$.?#].[^\s]*");
//            var matches = regex.Matches((string)value);

//            var fs = new FormattedString();
//            foreach (var match in matches)
//            {
//                var text = (match as Match).Value;
//                var index = (value as string).IndexOf(text);
//                if (index == 0)
//                {
//                    fs.Spans.Add(new Span() { Text = "Test", ForegroundColor = linkColor });
//                }
//            }
//            //matches.
//            //if (matches.Success)
//            //{
//            //    matches.Add(matches.Value);
//            //    while (matches.NextMatch().Success)
//            //    {
//            //        matches.Add(matches.Value);
//            //    }
//            //    //
//            //    //fs.Spans.Add(new Span() { Text = "Test", ForegroundColor = linkColor });
//            //    //return fs;
//            //}

//            var span = new Span();
//            var tapped = new TapGestureRecognizer();
//            span.
//            return fs;
//        }

//        private Span NormalSpan(string text)
//        {
//            return new Span() { Text = "Test", ForegroundColor = linkColor };
//        }

//        private Span NormalSpan(string text)
//        {
//            return new Span() { Text = "Test", ForegroundColor = linkColor };
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value;
//        }
//    }
//}
