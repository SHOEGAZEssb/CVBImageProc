using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Binarise"/> processor.
  /// </summary>
  class BinariseViewModel : FullProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Threshold of the binarization.
    /// Values &lt; will be set to 0.
    /// Values &gt;= will be set to 255.
    /// </summary>
    public int Threshold
    {
      get => _processor.Threshold;
      set
      {
        if (Threshold != value)
        {
          _processor.Threshold = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The actual processor.
    /// </summary>
    private readonly Binarise _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The binarise processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="processor"/> is null.</exception>
    public BinariseViewModel(Binarise processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}