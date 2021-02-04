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
    public IProcessor Parent
    {
      get => _parent;
      set
      {
        _parent = value;

        object target = Parent;
        PropertyInfo pi = null;
        foreach (var prop in PropertyName.Split('.'))
        {
          pi = target.GetType().GetProperty(prop);

          if(prop != PropertyName.Split('.').Last())
            target = pi.GetValue(target, null);
        }

        _property = pi;
        _target = target;
      }
    }
    private IProcessor _parent;

    public Type PropertyType => _property.PropertyType;

    #endregion Properties

    #region Member

    private int _currentTicks;
    private int _ticksToUpdate;

    /// <summary>
    /// The property to set.
    /// </summary>
    private PropertyInfo _property;

    /// <summary>
    /// Target object whose property to set.
    /// This does not necessarily need to be the
    /// <see cref="Parent"/>, as nested properties
    /// can be possible. (for example ValueProviders)
    /// </summary>
    private object _target;

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
      if(++_currentTicks == _ticksToUpdate)
      {
        UpdateValue();
        _currentTicks = 0;
      }
    }

    private void UpdateValue()
    {
      _property.SetValue(_target, ValueProvider.ProvideNextValue());
      ValueUpdated?.Invoke(this, EventArgs.Empty);
    }
  }
}