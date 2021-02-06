using CVBImageProcLib.Processing.Automation;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.ValueProvider
{
  /// <summary>
  /// Object providing byte values to processors.
  /// </summary>
  [DataContract]
  public class ByteValueProvider : AutomatableObjectBase, IValueProvider<byte>
  {
    #region Properties

    /// <summary>
    /// The configured value to provide in
    /// normal mode.
    /// </summary>
    [DataMember]
    public byte FixedValue { get; set; }

    /// <summary>
    /// The minimum possible byte value.
    /// </summary>
    [DataMember]
    public byte MinValue { get; private set; }

    /// <summary>
    /// The maximum possible byte value.
    /// </summary>
    [DataMember]
    public byte MaxValue { get; private set; }

    /// <summary>
    /// If true, randomizes the bytes to provide.
    /// </summary>
    [DataMember]
    public bool Randomize { get; set; }

    /// <summary>
    /// The minimum byte to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    [DataMember]
    public byte MinRandomValue { get; set; }

    /// <summary>
    /// The maximum byte to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    [DataMember]
    public byte MaxRandomValue { get; set; }

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
    public ByteValueProvider(byte min, byte max)
    {
      MinValue = min;
      MaxValue = max;
      MinRandomValue = MinValue;
      MaxRandomValue = MaxValue;
    }

    #endregion Construction

    /// <summary>
    /// Provides the next byte.
    /// </summary>
    /// <returns>Byte based on the current configuration.</returns>
    public byte Provide()
    {
      return Randomize ? (byte)_rng.Next(MinRandomValue, MaxRandomValue + 1) : FixedValue;
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