using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Object providing byte values to processors.
  /// </summary>
  [DataContract]
  class ByteValueProvider
  {
    #region Properties

    /// <summary>
    /// The configured value to provide in
    /// normal mode.
    /// </summary>
    [DataMember]
    public byte Value { get; set; }

    /// <summary>
    /// The minimum possible byte value.
    /// </summary>
    [DataMember]
    public byte MinByte { get; private set; }

    /// <summary>
    /// The maximum possible byte value.
    /// </summary>
    [DataMember]
    public byte MaxByte { get; private set; }

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
    public byte MinRandomByte { get; set; }

    /// <summary>
    /// The maximum byte to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    [DataMember]
    public byte MaxRandomByte { get; set; }

    #endregion Properties

    #region Member

    /// <summary>
    /// Random number generator for the
    /// <see cref="Randomize"/> mode.
    /// </summary>
    private readonly Random _rng = new Random(DateTime.Now.Ticks.GetHashCode());

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="min">Minimum possible value.</param>
    /// <param name="max">Maximum possible value.</param>
    public ByteValueProvider(byte min, byte max)
    {
      MinByte = min;
      MaxByte = max;
      MinRandomByte = MinByte;
      MaxRandomByte = MaxByte;
    }

    #endregion Construction

    /// <summary>
    /// Provides the next byte.
    /// </summary>
    /// <returns>Byte based on the current configuration.</returns>
    public byte Provide()
    {
      return Randomize ? (byte)_rng.Next(MinRandomByte, MaxRandomByte) : Value;
    }
  }
}