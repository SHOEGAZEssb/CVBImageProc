using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Automation
{
  [DataContract]
  public class AutomatableObjectBase : IAutomatable
  {
    #region Properties

    /// <summary>
    /// Configured automations.
    /// </summary>
    public IReadOnlyList<IPropertyAutomation> Automations => _automations.AsReadOnly();
    [DataMember]
    private List<IPropertyAutomation> _automations = new List<IPropertyAutomation>();

    #endregion Properties

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      foreach (var automation in Automations)
        automation.Parent = this;
    }

    public void AddAutomation(IPropertyAutomation automation)
    {
      if (automation == null)
        throw new ArgumentNullException(nameof(automation));
      if (_automations.Contains(automation))
        throw new ArgumentException("Automation already added", nameof(automation));

      _automations.Add(automation);
    }

    public void RemoveAutomation(IPropertyAutomation automation)
    {
      if (automation == null)
        throw new ArgumentNullException(nameof(automation));
      if (!_automations.Contains(automation))
        throw new ArgumentException("Automation not added", nameof(automation));

      _automations.Remove(automation);
    }
  }
}
