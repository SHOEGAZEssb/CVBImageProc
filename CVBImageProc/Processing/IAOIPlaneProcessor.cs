using CVBImageProc.Processing.PixelFilter;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Grouping interface for processors that support processing
  /// individual regions and planes.
  /// </summary>
  interface IAOIPlaneProcessor : ICanProcessIndividualRegions, IProcessIndividualPlanes, IProcessor
  {
  }
}
