using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SamplesModel.Enum;

namespace SampleBrowser.Common
{
    [ValueConversion(typeof(Language), typeof(string))]
    public class LanguageToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                return null;
            switch ((Language)value)
            {
                case Language.Cpp:
                    return "C++";
                case Language.CSharp:
                    return "C#";
                case Language.FSharp:
                    return "F#";
                case Language.VisualBasic:
                    return "VB";
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
