namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a pixel filter that does
  /// not need any external value to check, but
  /// rather, is dependent on its own internal state.
  /// </summary>
  public interface IPixelAutoFilter : IPixelFilter
  {
    /// <summary>
    /// Checks if the condition of the filter is
    /// fulfilled.
    /// </summary>
    /// <returns>True if the condition is fulfilled,
    /// otherwise false.</returns>
    bool Check();
  }
}