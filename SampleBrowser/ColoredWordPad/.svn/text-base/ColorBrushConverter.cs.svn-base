using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ColoredWordPad
{
    class ColorBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) { return  new SolidColorBrush(Colors.Transparent ); }
            try
            {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }
            catch
            {
              return  new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;
            if (brush  == null)
            {
                return Colors.Transparent;
            }
            else
            {
                return brush.Color;

            }
           
        }

        #endregion
    }
}
