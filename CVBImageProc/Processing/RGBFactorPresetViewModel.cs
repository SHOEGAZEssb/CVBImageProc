using CVBImageProc.MVVM;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for a <see cref="RGBFactorPreset"/>.
  /// </summary>
  internal sealed class RGBFactorPresetViewModel : ViewModelBase
  {
    #region Properties

    /// <summary>
    /// Name of the preset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The preset.
    /// </summary>
    public RGBFactorPreset Preset { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the preset.</param>
    /// <param name="preset">The preset.</param>
    public RGBFactorPresetViewModel(string name, RGBFactorPreset preset)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Preset = preset;
    }

    #endregion Construction
  }
}