using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for a <see cref="Rotate"/> processor.
  /// </summary>
  class RotateViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Degree by which to rotate.
    /// </summary>
    public double Degree
    {
      get => _processor.Angle.Deg;
      set
      {
        if (Degree != value)
        {
          _processor.Angle = Angle.FromDegrees(value);
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// If true, fits the rotated image
    /// in the new image.
    /// If false, new image size will be
    /// equal to the size of the input image.
    /// </summary>
    public bool FitImage
    {
      get => _processor.FitImage;
      set
      {
        if(FitImage != value)
        {
          _processor.FitImage = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Fill value for "empty" pixels.
    /// </summary>
    public byte FillValue
    {
      get => _processor.FillValue;
      set
      {
        if (FillValue != value)
        {
          _processor.FillValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly Rotate _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The actual processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public RotateViewModel(Rotate processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
    }

    #endregion Construction

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SubVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}