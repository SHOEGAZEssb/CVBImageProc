using System;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Attribute marking a filter with custom settings.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class CustomFilterSettingsAttribute : Attribute
  { }
}