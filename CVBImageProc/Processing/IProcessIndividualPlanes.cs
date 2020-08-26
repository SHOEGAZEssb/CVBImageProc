namespace CVBImageProc.Processing
{
  /// <summary>
  /// Interface for a processor that can process individual planes.
  /// </summary>
  interface IProcessIndividualPlanes : IProcessor
  {
    /// <summary>
    /// Index of the plane to process.
    /// </summary>
    int PlaneIndex { get; set; }
  }
}