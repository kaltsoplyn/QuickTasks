using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuickTasks
{
    public class LogToInfoSignConverter : IValueConverter
    {
        public enum Log { Info, Warning, Error };

        public LogToInfoSignConverter() {}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Log.Info:
                    return "INFO";
                case Log.Warning:
                    return "WARNING";
                case Log.Error:
                    return "ERROR";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
