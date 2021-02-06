using CVBImageProcLib.Processing.Automation;
using CVBImageProcLib.Processing.PixelFilter;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Grouping interface for processors that support processing
  /// individual regions and planes.
  /// </summary>
  public interface IAOIPlaneProcessor : ICanProcessIndividualRegions, IProcessIndividualPlanes, IAutomatableProcessor
  { }
}
