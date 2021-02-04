using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLib.Processing.Automation
{
  [DataContract]
  public class PropertyAutomator
  {
    #region Properties

    public static int UpdatesPerSecond { get; } = 100;

    public bool Enabled
    {
      get => _enabled;
      set
      {
        _enabled = value;
        if (Enabled)
          UpdateTask().Forget();
      }
    }
    private bool _enabled;

    public List<IPropertyAutomation> Automations { get; private set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PropertyAutomator()
    {
      Automations = new List<IPropertyAutomation>();
    }

    #endregion Construction

    private async Task UpdateTask()
    {
      while (Enabled)
      {
        await Task.Delay(10);
        foreach (var automation in Automations)
          automation.Update();
      }
    }
  }
}