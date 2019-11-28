using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Gain"/> processor.
  /// </summary>
  class GainViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    public bool WrapAround
    {
      get => _processor.WrapAround;
      set
      {
        if (WrapAround != value)
        {
          _processor.WrapAround = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// The gain value to apply.
    /// </summary>
    public double GainValue
    {
      get => _processor.GainValue;
      set
      {
        if(GainValue != value)
        {
          _processor.GainValue = value;
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
    /// The gain processor.
    /// </summary>
    private readonly Gain _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The gain processor.</param>
    public GainViewModel(Gain processor)
      : base(processor)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor.PixelFilter);
      PixelFilterChainVM.SettingsChanged += PixelFilterChainVM_SettingsChanged;
    }

    #endregion Construction

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void PixelFilterChainVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}