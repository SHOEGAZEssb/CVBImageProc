using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.ValueProvider
{
  /// <summary>
  /// Object providing byte values to processors.
  /// </summary>
  [DataContract]
  public class ByteValueProvider : IValueProvider<byte>
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
      return Randomize ? (byte)ThreadSafeRandom.Next(MinRandomValue, MaxRandomValue + 1) : FixedValue;
    }
  }
}