using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SamplesModel.Enum;

namespace SampleBrowser.Common
{
    [ValueConversion(typeof(Technology), typeof(string))]
    public class TechnologyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                return null;
            switch ((Technology)value)
            {
                case Technology.ASPNET:
                    return "ASP.NET";
                case Technology.WindowsForms:
                    return "Windows Forms";
                case Technology.Windows7:
                    return "Windows 7";
                case Technology.DataPlatform:
                    return "Data Platform";
                case Technology.WindowsUI:
                    return "Windows UI";
                case Technology.WindowsShell:
                    return "Windows Shell";
                case Technology.IPCandRPC:
                    return "IPC and RPC";
                case Technology.FileSystem:
                    return "File System";
                case Technology.WindowsService:
                    return "Windows Service";
                case Technology.WindowsPhone:
                    return "Windows Phone";
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
