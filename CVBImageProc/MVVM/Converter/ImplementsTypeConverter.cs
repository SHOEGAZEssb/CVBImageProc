using System;
using System.Globalization;
using System.Windows.Data;

namespace CVBImageProc.MVVM.Converter
{
  /// <summary>
  /// Checks if an object implements a given type.
  /// </summary>
  public class ImplementsTypeConverter : IValueConverter
  {
    /// <summary>
    /// Checks if the given <paramref name="value"/> implements
    /// the <paramref name="parameter"/> type.
    /// </summary>
    /// <param name="value">Object to check.</param>
    /// <param name="targetType">Target type of the resulting value.</param>
    /// <param name="parameter">Contains the type to check for.</param>
    /// <param name="culture">Conversion culture.</param>
    /// <returns>True if the object implements the type,
    /// otherwise false.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (value == null || parameter == null || !(parameter is Type type)) ? false : type.IsAssignableFrom(value.GetType());
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <param name="value">Object to check.</param>
    /// <param name="targetType">Target type of the resulting value.</param>
    /// <param name="parameter">Contains the type to check for.</param>
    /// <param name="culture">Conversion culture.</param>
    /// <returns>Not implemented.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}