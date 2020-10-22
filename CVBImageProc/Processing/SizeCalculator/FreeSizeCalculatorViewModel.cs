using CVBImageProcLib.Processing.SizeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.SizeCalculator
{
  class FreeSizeCalculatorViewModel : SizeCalculatorViewModelBase
  {
    #region Properties

    public int Width
    {
      get => _sizeCalculator.Width;
      set
      {
        if(Width != value)
        {
          _sizeCalculator.Width = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    public int Height
    {
      get => _sizeCalculator.Height;
      set
      {
        if (Width != value)
        {
          _sizeCalculator.Height = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    private readonly FreeSizeCalculator _sizeCalculator;

    #endregion Member

    #region Construction

    public FreeSizeCalculatorViewModel(FreeSizeCalculator sizeCalculator)
      : base(sizeCalculator)
    {
      _sizeCalculator = sizeCalculator;
    }

    #endregion Construction
  }
}
