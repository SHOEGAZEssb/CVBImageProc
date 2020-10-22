using CVBImageProcLib.Processing.SizeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.SizeCalculator
{
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
        if(Percentage != value)
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