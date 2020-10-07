using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="MonoToMultiplane"/> processor.
  /// </summary>
  class MonoToMultiplaneViewModel : ProcessorViewModel, IHasSettings
  {
    #region Properties

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    /// <summary>
    /// Amount of planes in the new image.
    /// </summary>
    public int NumPlanes
    {
      get => _processor.NumPlanes;
      set
      {
        if (NumPlanes != value)
        {
          _processor.NumPlanes = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly MonoToMultiplane _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public MonoToMultiplaneViewModel(MonoToMultiplane processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}
