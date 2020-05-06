using System;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// Attribute marking a filter with custom settings.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  sealed class CustomFilterSettingsAttribute : Attribute
  { }
}