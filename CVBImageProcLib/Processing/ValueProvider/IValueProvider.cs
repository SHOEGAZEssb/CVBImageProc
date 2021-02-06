using CVBImageProcLib.Processing.Automation;

namespace CVBImageProcLib.Processing.ValueProvider
{
  /// <summary>
  /// Interface for an object providing values.
  /// </summary>
  /// <typeparam name="T">Type of value to provide.</typeparam>
  public interface IValueProvider<T> : IAutomatable where T : struct
  {
    #region Properties

    /// <summary>
    /// The configured value to provide in
    /// normal mode.
    /// </summary>
    T FixedValue { get; set; }

    /// <summary>
    /// The minimum possible value.
    /// </summary>
    T MinValue { get; }

    /// <summary>
    /// The maximum possible value.
    /// </summary>
    T MaxValue { get; }

    /// <summary>
    /// If true, randomizes the values to provide.
    /// </summary>
    bool Randomize { get; set; }

    /// <summary>
    /// The minimum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    T MinRandomValue { get; set; }

    /// <summary>
    /// The maximum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    T MaxRandomValue { get; set; }

    #endregion Properties

    /// <summary>
    /// Provides the next value.
    /// </summary>
    /// <returns>Value based on the current configuration.</returns>
    T Provide();
  }
}