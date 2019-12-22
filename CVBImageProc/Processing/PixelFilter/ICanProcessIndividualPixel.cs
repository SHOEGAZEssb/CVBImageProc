using Stemmer.Cvb;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Interface for a processor that can process
  /// individual pixels.
  /// </summary>
  interface ICanProcessIndividualPixel : IProcessor
  {
    /// <summary>
    /// Pixel filter chain of the processor.
    /// </summary>
    PixelFilterChain PixelFilter { get; }

    bool UseAOI { get; set; }

    Rect AOI { get; set; }
  }
}