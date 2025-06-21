using System;
using System.Globalization;
using System.Windows.Data;

namespace PowerTrackWPF.Converters
{
    public class BooleanToOppositeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b ? "Ver más" : "Ver menos";
            }
            return "Ver más";
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}