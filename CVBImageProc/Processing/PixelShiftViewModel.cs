using CVBImageProc.Processing.PixelFilter;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  class PixelShiftViewModel : AOIPlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// The shift in X-direction.
    /// </summary>
    public int ShiftX
    {
      get => _processor.ShiftX;
      set
      {
        if(ShiftX != value)
        {
          _processor.ShiftX = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// The shift in Y-direction.
    /// </summary>
    public int ShiftY
    {
      get => _processor.ShiftY;
      set
      {
        if (ShiftY != value)
        {
          _processor.ShiftY = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    public bool Wrap
    {
      get => _processor.Wrap;
      set
      {
        if(Wrap != value)
        {
          _processor.Wrap = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    public byte FillValue
    {
      get => _processor.FillValue;
      set
      {
        if(FillValue != value)
        {
          _processor.FillValue = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
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
    private readonly PixelShift _processor;

    #endregion Member

    #region Construction

    public PixelShiftViewModel(PixelShift processor, bool isActive)
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