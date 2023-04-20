using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace BeginSEO.Convert
{
    public class BoolToColor : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 根据变量的值返回对应的颜色
            if (value == null || (bool)value == false)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#909399"));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007FFF"));
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
