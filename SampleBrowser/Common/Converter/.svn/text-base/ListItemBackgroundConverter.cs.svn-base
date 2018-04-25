using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Drawing;


namespace SampleBrowser.Common
{
    public class ListItemBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = (ListBoxItem)value;
            ListBox listBox = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
            // Get the index of a ListBoxItem 
            int index = listBox.ItemContainerGenerator.IndexFromContainer(item);

            if (index % 2 == 0)
            {
                return Brushes.White;
            }
            else
            {
                return Brushes.LightGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
