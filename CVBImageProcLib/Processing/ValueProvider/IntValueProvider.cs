using CVBImageProcLib.Processing.Automation;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.ValueProvider
{
  /// <summary>
  /// Object for providing int values;
  /// </summary>
  [DataContract]
  public class IntValueProvider : AutomatableObjectBase, IValueProvider<int>
  {
    #region Properties

    /// <summary>
    /// The configured value to provide in
    /// normal mode.
    /// </summary>
    [DataMember]
    public int FixedValue { get; set; }

    /// <summary>
    /// The minimum possible value.
    /// </summary>
    [DataMember]
    public int MinValue { get; private set; }

    /// <summary>
    /// The maximum possible value.
    /// </summary>
    [DataMember]
    public int MaxValue { get; private set; }

    /// <summary>
    /// If true, randomizes the values to provide.
    /// </summary>
    [DataMember]
    public bool Randomize { get; set; }

    /// <summary>
    /// The minimum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    [DataMember]
    public int MinRandomValue { get; set; }

    /// <summary>
    /// The maximum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    [DataMember]
    public int MaxRandomValue { get; set; }

    #endregion Properties

    #region Member

    /// <summary>
    /// Random number generator for the
    /// <see cref="Randomize"/> mode.
    /// </summary>
    private Random _rng = new Random(DateTime.Now.Ticks.GetHashCode());

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="min">Minimum possible value.</param>
    /// <param name="max">Maximum possible value.</param>
    public IntValueProvider(int min, int max)
    {
      MinValue = min;
      MaxValue = max;
      MinRandomValue = MinValue;
      MaxRandomValue = MaxValue;
    }

    #endregion Construction

    /// <summary>
    /// Provides the next value.
    /// </summary>
    /// <returns>Value based on the current configuration.</returns>
    public int Provide()
    {
      return Randomize ? _rng.Next(MinRandomValue, MaxRandomValue + 1) : FixedValue;
    }

    /// <summary>
    /// Creates the <see cref="_rng"/> object
    /// when deserializing.
    /// </summary>
    /// <param name="context">Ignored.</param>
    [OnDeserialized]
    internal void OnDeserialized(StreamingContext context)
    {
      _rng = new Random(DateTime.Now.Ticks.GetHashCode());
    }
  }
}