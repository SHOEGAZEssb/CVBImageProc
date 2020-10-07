namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Interface for a processor that can process individual planes.
  /// </summary>
  public interface IProcessIndividualPlanes : IProcessor
  {
    /// <summary>
    /// Index of the plane to process.
    /// </summary>
    int PlaneIndex { get; set; }
  }
}