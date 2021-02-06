using CVBImageProc.MVVM;
using CVBImageProcLib.Processing;
using CVBImageProcLib.Processing.Automation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace CVBImageProc.Processing.Automation
{
  /// <summary>
  /// Base ViewModel for automatable processors.
  /// </summary>
  abstract class AutomatableProcessorViewModelBase : ProcessorViewModel, IHasSettings, IAutomatableObjectViewModel
  {
    #region IHasSettings Implementation

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region Properties

    public event EventHandler<PropertyAutomationEventArgs> AutomationAdded;

    public event EventHandler<PropertyAutomationEventArgs> AutomationRemoved;

    public ObservableCollection<PropertyAutomationViewModel> Automations => _automatableObjectVM.Automations;

    /// <summary>
    /// All automations of this processor and all
    /// its automatable sub-object.
    /// </summary>
    public virtual CompositeObservableCollection<PropertyAutomationViewModel> AllAutomations { get; } // todo: make abstract once every processor is automatable.

    public PropertyAutomationViewModel SelectedAutomation
    {
      get => _selectedAutomation;
      set
      {
        if(SelectedAutomation != value)
        {
          _selectedAutomation = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private PropertyAutomationViewModel _selectedAutomation;

    public ICommand AddAutomationCommand => _automatableObjectVM.AddAutomationCommand;

    public ICommand RemoveAutomationCommand => _automatableObjectVM.RemoveAutomationCommand;

    #endregion Properties

    #region Member

    private readonly IAutomatableObjectViewModel _automatableObjectVM;

    #endregion Member

    protected AutomatableProcessorViewModelBase(IProcessor processor, bool isActive) 
      : base(processor, isActive)
    {
      _automatableObjectVM = new AutomatableObjectViewModelBase(processor as IAutomatableProcessor); // todo: type in constructor (once every processor has been adapted)
      _automatableObjectVM.AutomationAdded += AutomatableObjectVM_AutomationAdded;
      _automatableObjectVM.AutomationRemoved += AutomatableObjectVM_AutomationRemoved;
      _automatableObjectVM.PropertyChanged += AutomatableObjectVM_PropertyChanged;
      _automatableObjectVM.SettingsChanged += AutomatableObjectVM_SettingsChanged;
    }

    private void AutomatableObjectVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }

    private void AutomatableObjectVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      NotifyOfPropertyChange(e.PropertyName);
    }

    private void AutomatableObjectVM_AutomationRemoved(object sender, PropertyAutomationEventArgs e)
    {
      OnAutomationRemoved(e);
    }

    protected void OnAutomationRemoved(PropertyAutomationEventArgs e)
    {
      AutomationRemoved?.Invoke(this, e);
    }

    private void AutomatableObjectVM_AutomationAdded(object sender, PropertyAutomationEventArgs e)
    {
      OnAutomationAdded(e);
    }

    protected void OnAutomationAdded(PropertyAutomationEventArgs e)
    {
      AutomationAdded?.Invoke(this, e);
    }

    protected void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}