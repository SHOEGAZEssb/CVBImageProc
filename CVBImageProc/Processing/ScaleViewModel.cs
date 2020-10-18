using CVBImageProcLib.Processing;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Scale"/> processor.
  /// </summary>
  class ScaleViewModel : ProcessorViewModel, IHasSettings
  {
    #region Properties

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    /// <summary>
    /// Target width.
    /// </summary>
    public int NewWidth
    {
      get => _processor.NewSize.Width;
      set
      {
        if (NewWidth != value)
        {
          _processor.NewSize = new Size2D(value, _processor.NewSize.Height);
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Target height.
    /// </summary>
    public int NewHeight
    {
      get => _processor.NewSize.Height;
      set
      {
        if (NewHeight != value)
        {
          _processor.NewSize = new Size2D(_processor.NewSize.Width, value);
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Algorithm to use for scaling.
    /// </summary>
    public ScaleMode Mode
    {
      get => _processor.Mode;
      set
      {
        if(Mode != value)
        {
          _processor.Mode = value;
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
    private readonly Scale _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public ScaleViewModel(Scale processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}