using Stemmer.Cvb;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Automation
{
  /// <summary>
  /// Base class for a processor whose properties can
  /// be automated.
  /// </summary>
  [DataContract]
  public abstract class AutomatableProcessorBase : IProcessor
  {
    #region Properties

    /// <summary>
    /// Name of this processor.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Configured automations.
    /// </summary>
    [DataMember]
    public List<IPropertyAutomation> Automations { get; private set; } = new List<IPropertyAutomation>();

    #endregion Properties

    public abstract Image Process(Image inputImage);

    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      foreach (var automation in Automations)
        automation.Parent = this;
    }
  }
}