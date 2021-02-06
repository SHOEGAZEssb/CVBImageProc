using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.Automation;
using System;

namespace CVBImageProc.Processing.Automation
{
  abstract class PropertyAutomationViewModel : ViewModelBase
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

    protected PropertyAutomationViewModel(IPropertyAutomation automation)
    {
      _automation = automation ?? throw new ArgumentNullException(nameof(automation));
    }

    #endregion Construction
  }
}