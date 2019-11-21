using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Binarise"/> processor.
  /// </summary>
  class BinariseViewModel : ViewModelBase, IProcessorViewModel, IHasSettings
  {
    #region IProcessorViewModel Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => _processor.Name;

    #endregion IProcessorViewModel Implementation

    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region Properties

    public int Threshold
    {
      get => _processor.Threshold;
      set
      {
        if(Threshold != value)
        {
          _processor.Threshold = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    public int MaxThreshold => Binarise.MAXTHRESHOLD;

    public int MinThreshold => Binarise.MINTHRESHOLD;

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
    /// <exception cref="ArgumentNullException">When <paramref name="processor"/> is null.</exception>
    public BinariseViewModel(Binarise processor)
    {
      _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    #endregion Construction
  }
}
