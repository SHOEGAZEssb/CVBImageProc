using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Interface for an object that has settings.
  /// </summary>
  interface IHasSettings
  {
    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    event EventHandler SettingsChanged;
  }
}