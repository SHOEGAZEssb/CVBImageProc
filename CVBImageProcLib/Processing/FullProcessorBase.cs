using CVBImageProcLib.Processing.PixelFilter;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Grouping interface for a processor
  /// with full configuration capabilities.
  /// </summary>
  [DataContract]
  public abstract class FullProcessorBase : AOIPlaneProcessorBase, IFullProcessor
  {
    #region IFullProcessor Implementation

    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion IFullProcessor Implementation
  }
}