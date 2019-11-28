﻿using CVBImageProc.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Binarise"/> processor.
  /// </summary>
  class BinariseViewModel : ProcessorViewModel, IHasSettings
  {
    #region IHasSettings Implementation

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

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
        if(Threshold != value)
        {
          _processor.Threshold = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Max value of the <see cref="Threshold"/>.
    /// </summary>
    public int MaxThreshold => Binarise.MAXTHRESHOLD;

    /// <summary>
    /// Min value of the <see cref="Threshold"/>.
    /// </summary>
    public int MinThreshold => Binarise.MINTHRESHOLD;

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

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
      : base(processor)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor.PixelFilter);
      PixelFilterChainVM.SettingsChanged += PixelFilterChainVM_SettingsChanged;
    }

    #endregion Construction

    private void PixelFilterChainVM_SettingsChanged(object sender, EventArgs e)
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
