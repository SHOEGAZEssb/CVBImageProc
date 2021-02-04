using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLib.Processing.Automation.ValueProvider
{
  [DataContract]
  public class IntUpAutomationValueProvider : IAutomationValueProvider
  {
    #region Properties

    [DataMember]
    public int Minimum { get; private set; }

    [DataMember]
    public int Maximum { get; private set; }

    [DataMember]
    public int LowerBound
    {
      get => _lowerBound;
      set
      {
        if (value > UpperBound)
          throw new ArgumentOutOfRangeException($"{nameof(LowerBound)} can't be larger than {nameof(UpperBound)}");
        _lowerBound = value;
      }
    }
    private int _lowerBound;

    [DataMember]
    public int UpperBound
    {
      get => _upperBound;
      set
      {
        if (value < LowerBound)
          throw new ArgumentOutOfRangeException($"{nameof(UpperBound)} can't be smaller than {nameof(LowerBound)}");
        _upperBound = value;
      }
    }
    private int _upperBound;

    [DataMember]
    public int Step { get; private set; }

    #endregion Properties

    #region Member

    private int _value;

    #endregion Member

    #region Construction

    public IntUpAutomationValueProvider(int minimum, int maximum)
    {
      if (minimum > maximum)
        throw new ArgumentOutOfRangeException($"{nameof(minimum)} can't be smaller than {nameof(maximum)}");

      Minimum = minimum;
      Maximum = maximum;
      LowerBound = Minimum;
      UpperBound = Maximum;
      Step = 10;
    }

    #endregion Construction

    public object ProvideNextValue()
    {
      int ret = _value;
      int newVal = _value + Step;
      if (newVal > UpperBound)
        newVal = LowerBound;
      _value = newVal;

      return ret;
    }
  }
}
