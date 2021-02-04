using CVBImageProcLib.Processing.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.Automation
{
  class PropertyAutomationEventArgs : EventArgs
  {
    #region Properties

    public IPropertyAutomation Automation { get; }

    #endregion Properties

    #region Construction

    public PropertyAutomationEventArgs(IPropertyAutomation automation)
    {
      Automation = automation ?? throw new ArgumentNullException(nameof(automation));
    }

    #endregion Construction
  }
}
