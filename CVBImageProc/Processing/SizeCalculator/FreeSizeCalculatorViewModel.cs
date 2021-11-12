using CVBImageProcLib.Processing.SizeCalculator;

namespace CVBImageProc.Processing.SizeCalculator
{
  /// <summary>
  /// ViewModel for the <see cref="FreeSizeCalculator"/>.
  /// </summary>
  internal class FreeSizeCalculatorViewModel : SizeCalculatorViewModelBase
  {
    #region Properties

    /// <summary>
    /// The new width of the size.
    /// </summary>
    public int Width
    {
      get => _sizeCalculator.Width;
      set
      {
        if (Width != value)
        {
          _sizeCalculator.Width = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// The new height of the size.
    /// </summary>
    public int Height
    {
      get => _sizeCalculator.Height;
      set
      {
        if (Height != value)
        {
          _sizeCalculator.Height = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The size calculator.
    /// </summary>
    private readonly FreeSizeCalculator _sizeCalculator;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sizeCalculator">The size calculator.</param>
    public FreeSizeCalculatorViewModel(FreeSizeCalculator sizeCalculator)
      : base(sizeCalculator)
    {
      _sizeCalculator = sizeCalculator;
    }

    #endregion Construction
  }
}