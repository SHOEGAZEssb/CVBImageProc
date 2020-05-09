using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Interface for an object that has settings.
  /// </summary>
  public interface IHasSettings
  {
    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    event EventHandler SettingsChanged;
  }
}