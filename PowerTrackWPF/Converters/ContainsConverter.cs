using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PowerTrackWPF.Converters
{
    public class ContainsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is ObservableCollection<int> selected && values[1] is int id)
            {
                return selected.Contains(id);
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // No usado directamente aquí
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}