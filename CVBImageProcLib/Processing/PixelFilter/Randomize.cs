using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
  /// <summary>
  /// Pixel filter than randomly passes.
  /// </summary>
  [DataContract]
  public sealed class Randomize : IPixelAutoFilter
  {
    #region Properties

    /// <summary>
    /// Name of this pixel filter.
    /// </summary>
    public string Name => "Randomize";

    /// <summary>
    /// If true, inverts the logic of the filter.
    /// </summary>
    [DataMember]
    public bool Invert { get; set; }

    /// <summary>
    /// Chance that the check passes.
    /// </summary>
    [DataMember]
    public double Chance
    {
      get => _chance;
      set
      {
        if (Chance != value)
        {
          if (value > 1.0)
            value = 1.0;
          else if (value < 0.0)
            value = 0.0;

          _chance = value;
        }
      }
    }
    private double _chance;

    #endregion Properties

    /// <summary>
    /// Checks if the condition of the filter is
    /// fulfilled.
    /// </summary>
    /// <returns>True if the condition is fulfilled,
    /// otherwise false.</returns>
    public bool Check()
    {
      return Invert ? ThreadSafeRandom.NextDouble() >= Chance : ThreadSafeRandom.NextDouble() < Chance;
    }
  }
}