using CVBImageProc.MVVM;
using CVBImageProcLib.Processing;
using CVBImageProcLib.Processing.Automation;
using CVBImageProcLib.Processing.Automation.ValueProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVBImageProc.Processing.Automation
{
  class AutomatableProcessorViewModelBase : ProcessorViewModel, IHasSettings
  {
    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    protected void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion IHasSettings Implementation

    #region Properties

    public event EventHandler<PropertyAutomationEventArgs> AutomationAdded;

    public event EventHandler<PropertyAutomationEventArgs> AutomationRemoved;

    public ObservableCollection<PropertyAutomationViewModel> Automations { get; }

    public ICommand AddAutomationCommand { get; }

    #endregion Properties

    #region Member

    private readonly AutomatableProcessorBase _processor;

    #endregion Member

    public AutomatableProcessorViewModelBase(IProcessor processor, bool isActive) 
      : base(processor, isActive)
    {
      _processor = processor as AutomatableProcessorBase;
      Automations = new ObservableCollection<PropertyAutomationViewModel>();
      AddAutomationCommand = new DelegateCommand((s) => AddAutomation(s as string));
    }

    private void AddAutomation(string propertyName)
    {
      var automation = new PropertyAutomation(propertyName)
      {
        Parent = _processor,
      };
      automation.ValueProvider = GetDefaultValueProvider(automation.PropertyType);
      automation.UpdatesPerSecond = 50;

      automation.ValueUpdated += Automation_ValueUpdated;
      _processor.Automations.Add(automation);

      Automations.Add(new PropertyAutomationViewModel(automation));
      AutomationAdded?.Invoke(this, new PropertyAutomationEventArgs(automation));
    }

    private void Automation_ValueUpdated(object sender, EventArgs e)
    {
      NotifyOfPropertyChange((sender as IPropertyAutomation).PropertyName);
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
  }
}
