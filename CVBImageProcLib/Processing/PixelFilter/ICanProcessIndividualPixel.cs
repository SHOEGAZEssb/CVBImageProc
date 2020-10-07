namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a processor that can process
  /// individual pixels.
  /// </summary>
  public interface ICanProcessIndividualPixel
  {
    /// <summary>
    /// Pixel filter chain of the processor.
    /// </summary>
    PixelFilterChain PixelFilter { get; set; }
  }
}