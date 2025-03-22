using System.Windows.Data;
using System.Windows.Media;

namespace StudentMarksheet.Converters
{
    public class BooltoBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool && (bool)value
                ? new SolidColorBrush(Colors.LightGreen)
                : new SolidColorBrush(Colors.OrangeRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

    }
}
