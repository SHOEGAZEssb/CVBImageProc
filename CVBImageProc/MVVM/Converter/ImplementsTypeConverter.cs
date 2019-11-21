using System;
using System.Globalization;
using System.Windows.Data;

namespace CVBImageProc.MVVM.Converter
{
  class ImplementsTypeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || parameter == null || !(parameter is Type type))
        return false;

      return type.IsAssignableFrom(value.GetType());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
