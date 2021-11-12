using CVBImageProcLib.Processing.SizeCalculator;
using System;

namespace CVBImageProc.Processing.SizeCalculator
{
  /// <summary>
  /// Base class for all size calculators.
  /// </summary>
  internal abstract class SizeCalculatorViewModelBase : SettingsViewModelBase
  {
    #region Properties

    /// <summary>
    /// The size calculator.
    /// </summary>
    public ISizeCalculator SizeCalculator { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sizeCalculator">The size calculator.</param>
    protected SizeCalculatorViewModelBase(ISizeCalculator sizeCalculator)
    {
      SizeCalculator = sizeCalculator ?? throw new ArgumentNullException(nameof(sizeCalculator));
    }

    #endregion Construction
  }
}