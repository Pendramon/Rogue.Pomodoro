using System;
using System.Globalization;
using System.Windows.Data;

namespace Rogue.Pomodoro.WPF.Converters;

public class TimeSpanToHourMinuteSecondFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).ToString(@"hh\:mm\:ss");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}