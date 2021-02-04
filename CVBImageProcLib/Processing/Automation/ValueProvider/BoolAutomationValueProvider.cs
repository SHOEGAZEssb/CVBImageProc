using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Automation.ValueProvider
{
  [DataContract]
  public class BoolAutomationValueProvider : IAutomationValueProvider
  {
    #region Properties

    /// <summary>
    /// State of the next provide.
    /// </summary>
    [DataMember]
    public bool State { get; private set; }

    #endregion Properties

    public object ProvideNextValue()
    {
      bool ret = State;
      State = !State;
      return ret;
    }
  }
}
