using CVBImageProcLib.Processing.Automation.ValueProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLib.Processing.Automation
{
  [DataContract]
  public class PropertyAutomation : IPropertyAutomation
  {
    #region Properties

    public event EventHandler ValueUpdated;

    [DataMember]
    public double UpdatesPerSecond
    {
      get => _updatesPerSecond;
      set
      {
        _updatesPerSecond = value;
        _ticksToUpdate = (int)(PropertyAutomator.UpdatesPerSecond / UpdatesPerSecond);
      }
    }
    private double _updatesPerSecond;

    [DataMember]
    public string PropertyName { get; private set; }

    [DataMember]
    public IAutomationValueProvider ValueProvider { get; set; }

    /// <summary>
    /// The processor whose property
    /// (or nested property) to set.
    /// </summary>
    public IAutomatable Parent
    {
      get => _parent;
      set
      {
        _parent = value;
        _property = Parent.GetType().GetProperty(PropertyName); ;
      }
    }
    private IAutomatable _parent;

    public Type PropertyType => _property.PropertyType;

    #endregion Properties

    #region Member

    private int _currentTicks;
    private int _ticksToUpdate;

    /// <summary>
    /// The property to set.
    /// </summary>
    private PropertyInfo _property;

    #endregion Member

    #region Construction

    public PropertyAutomation(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentNullException(nameof(propertyName));
      PropertyName = propertyName;
      UpdatesPerSecond = 1;
    }

    #endregion Construction

    public void Update()
    {
      if(++_currentTicks >= _ticksToUpdate)
      {
        UpdateValue();
        _currentTicks = 0;
      }
    }

    private void UpdateValue()
    {
      _property.SetValue(Parent, ValueProvider.ProvideNextValue());
      ValueUpdated?.Invoke(this, EventArgs.Empty);
    }
  }
}