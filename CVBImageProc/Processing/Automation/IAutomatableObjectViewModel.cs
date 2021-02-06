using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVBImageProc.Processing.Automation
{
  interface IAutomatableObjectViewModel : IHasSettings, INotifyPropertyChanged
  {
    event EventHandler<PropertyAutomationEventArgs> AutomationAdded;

    event EventHandler<PropertyAutomationEventArgs> AutomationRemoved;

    ObservableCollection<PropertyAutomationViewModel> Automations { get; }

    ICommand AddAutomationCommand { get; }

    ICommand RemoveAutomationCommand { get; }
  }
}
