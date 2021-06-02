using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.ValueProvider
{
  /// <summary>
  /// Object for providing int values;
  /// </summary>
  [DataContract]
  public class IntValueProvider : IValueProvider<int>
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
      return Randomize ? ThreadSafeRandom.Next(MinRandomValue, MaxRandomValue + 1) : FixedValue;
    }
  }
}