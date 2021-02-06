using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.Automation;
using CVBImageProcLib.Processing.Automation.ValueProvider;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CVBImageProc.Processing.Automation
{
  class AutomatableObjectViewModelBase : SettingsViewModelBase, IAutomatableObjectViewModel
  {
    #region Properties

    public event EventHandler<PropertyAutomationEventArgs> AutomationAdded;

    public event EventHandler<PropertyAutomationEventArgs> AutomationRemoved;

    public ObservableCollection<PropertyAutomationViewModel> Automations { get; }

    public ICommand AddAutomationCommand { get; }

    public ICommand RemoveAutomationCommand { get; }

    #endregion Properties

    #region Member

    private readonly IAutomatable _automatableObject;

    #endregion Member

    #region Construction

    internal protected AutomatableObjectViewModelBase(IAutomatable automatableObject)
    {
      _automatableObject = automatableObject; // todo: null check
      Automations = new ObservableCollection<PropertyAutomationViewModel>();
      AddAutomationCommand = new DelegateCommand((s) => AddAutomation(s as string), (s) => !Automations.Any(a => a.PropertyName == s as string));
      RemoveAutomationCommand = new DelegateCommand((s) => RemoveAutomation(s as string), (s) => Automations.Any(a => a.PropertyName == s as string));
    }

    #endregion Construction

    private void AddAutomation(string propertyName)
    {
      var automation = new PropertyAutomation(propertyName)
      {
        Parent = _automatableObject
      };
      automation.ValueProvider = GetDefaultValueProvider(automation.PropertyType);
      automation.UpdatesPerSecond = 25;

      automation.ValueUpdated += Automation_ValueUpdated;
      _automatableObject.AddAutomation(automation);

      Automations.Add(MakeAutomationViewModel(automation));
      AutomationAdded?.Invoke(this, new PropertyAutomationEventArgs(automation));
      (AddAutomationCommand as DelegateCommand).OnCanExecuteChanged();
      (RemoveAutomationCommand as DelegateCommand).OnCanExecuteChanged();
    }

    private void RemoveAutomation(string propertyName)
    {
      var automation = _automatableObject.Automations.Where(a => a.PropertyName == propertyName).FirstOrDefault();
      if (automation == null)
        throw new ArgumentException("No automation with given name added", nameof(propertyName));

      automation.ValueUpdated -= Automation_ValueUpdated;
      _automatableObject.RemoveAutomation(automation);

      var automationVM = Automations.Where(a => a.PropertyName == automation.PropertyName).First();
      Automations.Remove(automationVM);
      AutomationRemoved?.Invoke(this, new PropertyAutomationEventArgs(automation));
      (AddAutomationCommand as DelegateCommand).OnCanExecuteChanged();
      (RemoveAutomationCommand as DelegateCommand).OnCanExecuteChanged();
    }

    private void Automation_ValueUpdated(object sender, EventArgs e)
    {
      NotifyOfPropertyChange((sender as IPropertyAutomation).PropertyName); // todo: possible performance improvement by removing cast
      OnSettingsChanged();
    }

    private static IAutomationValueProvider GetDefaultValueProvider(Type propertyType)
    {
      if (propertyType == typeof(bool))
        return new BoolAutomationValueProvider();
      else if (propertyType == typeof(int))
        return new IntUpAutomationValueProvider(0, 255);
      else
        throw new Exception();
    }

    private static PropertyAutomationViewModel MakeAutomationViewModel(IPropertyAutomation automation)
    {
      if (automation.PropertyType == typeof(bool))
        return new BoolPropertyAutomationViewModel(automation);
      else if (automation.PropertyType == typeof(int))
        return new IntPropertyAutomationViewModel(automation);
      else
        throw new ArgumentException("Unknown automation PropertyType", nameof(automation));
    }
  }
}
