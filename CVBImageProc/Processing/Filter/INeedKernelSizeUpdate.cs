namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// Interface for an object that needs to be
  /// notified when the kernel size of its
  /// filter changes.
  /// </summary>
  internal interface INeedKernelSizeUpdate
  {
    /// <summary>
    /// Tells this object to update its
    /// settings according to the kernel size
    /// of its filter.
    /// </summary>
    void Update();
  }
}