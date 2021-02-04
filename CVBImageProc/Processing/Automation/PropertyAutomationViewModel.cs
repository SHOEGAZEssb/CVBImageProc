using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.Automation
{
  class PropertyAutomationViewModel : ViewModelBase
  {
    #region Properties

    public string PropertyName => _automation.PropertyName;

    public double UpdatesPerSecond
    {
      get => _automation.UpdatesPerSecond;
      set
      {
        if(UpdatesPerSecond != value)
        {
          _automation.UpdatesPerSecond = value;
          NotifyOfPropertyChange();
        }
      }
    }

    #endregion Properties

    #region Member

    private readonly IPropertyAutomation _automation;

    #endregion Member

    #region Construction

    public PropertyAutomationViewModel(IPropertyAutomation automation)
    {
      _automation = automation ?? throw new ArgumentNullException(nameof(automation));
    }

    #endregion Construction
  }
}
