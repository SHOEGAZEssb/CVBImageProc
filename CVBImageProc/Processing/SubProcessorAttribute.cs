using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Attribute used to mark processors
  /// as a subprocessor. E.g. filters.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  class SubProcessorAttribute : Attribute
  { }
}