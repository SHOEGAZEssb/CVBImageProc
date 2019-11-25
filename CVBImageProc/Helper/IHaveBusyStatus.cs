namespace CVBImageProc.Helper
{
  /// <summary>
  /// Interface for displaying a
  /// busy status.
  /// </summary>
  interface IHaveBusyStatus
  {
    /// <summary>
    /// Gets if the object is currently busy.
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    /// Gets the current busy message.
    /// </summary>
    string BusyMessage { get; }
  }
}