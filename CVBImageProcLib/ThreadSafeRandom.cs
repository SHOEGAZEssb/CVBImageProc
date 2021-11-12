using System;

namespace CVBImageProcLib
{
  /// <summary>
  /// Thread safe random-number-generator.
  /// </summary>
  internal static class ThreadSafeRandom
  {
    private static readonly Random _global = new Random(DateTime.Now.Ticks.GetHashCode());
    [ThreadStatic]
    private static Random _local;

    /// <summary>
    /// Gets the next random double value.
    /// </summary>
    /// <returns>Random double value.</returns>
    public static double NextDouble()
    {
      if (_local == null)
      {
        lock (_global)
        {
          if (_local == null)
          {
            int seed = _global.Next();
            _local = new Random(seed);
          }
        }
      }

      return _local.NextDouble();
    }

    /// <summary>
    /// Gets the next integer double value.
    /// </summary>
    /// <param name="minValue">Inclusive minimum value.</param>
    /// <param name="maxValue">Inclusive maximum value.</param>
    /// <returns>Random integer number.</returns>
    public static int Next(int minValue, int maxValue)
    {
      if (_local == null)
      {
        lock (_global)
        {
          if (_local == null)
          {
            int seed = _global.Next();
            _local = new Random(seed);
          }
        }
      }

      return _local.Next(minValue, maxValue);
    }
  }
}