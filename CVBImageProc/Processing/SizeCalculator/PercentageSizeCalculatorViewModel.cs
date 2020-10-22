using CVBImageProcLib.Processing.SizeCalculator;

namespace CVBImageProc.Processing.SizeCalculator
{
  /// <summary>
  /// ViewModel for the <see cref="PercentageSizeCalculator"/>.
  /// </summary>
  class PercentageSizeCalculatorViewModel : SizeCalculatorViewModelBase
  {
    #region Properties

    /// <summary>
    /// The percentage to use of the input size.
    /// </summary>
    public double Percentage
    {
      get => _sizeCalculator.Percentage;
      set
      {
        if (Percentage != value)
        {
          _sizeCalculator.Percentage = value;
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
    private readonly PercentageSizeCalculator _sizeCalculator;

    #endregion Member

    public PercentageSizeCalculatorViewModel(PercentageSizeCalculator sizeCalculator)
      : base(sizeCalculator)
    {
      _sizeCalculator = sizeCalculator;
    }
  }
}