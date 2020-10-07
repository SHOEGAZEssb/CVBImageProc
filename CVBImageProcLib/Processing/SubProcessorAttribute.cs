using System;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Attribute used to mark processors
  /// as a subprocessor. E.g. filters.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class SubProcessorAttribute : Attribute
  { }
}