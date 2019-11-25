using CVBImageProc.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc
{
  /// <summary>
  /// ViewModel for the status bar.
  /// </summary>
  class StatusBarViewModel : ViewModelBase
  {
    #region Properties

    /// <summary>
    /// Current status message.
    /// </summary>
    public string StatusMessage
    {
      get => _statusMessage;
      set
      {
        if(StatusMessage != value)
        {
          _statusMessage = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private string _statusMessage;

    #endregion Properties
  }
}