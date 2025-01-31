using System.Windows;
using System.Windows.Data;

namespace R3Sample01.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool IsReversed { get; set; }
        public bool IsHold { get; set; }
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
            => (System.Convert.ToBoolean(value) ^ this.IsReversed) ? Visibility.Visible
                : this.IsHold ? Visibility.Hidden
                : Visibility.Collapsed;
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }
}
