using CVBImageProcLib.Processing.SizeCalculator;
using System;

namespace CVBImageProc.Processing.SizeCalculator
{
	/// <summary>
	/// Base class for all size calculators.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="sizeCalculator">The size calculator.</param>
	internal abstract class SizeCalculatorViewModelBase(ISizeCalculator sizeCalculator) : SettingsViewModelBase
	{
		#region Properties

		/// <summary>
		/// The size calculator.
		/// </summary>
		public ISizeCalculator SizeCalculator { get; } = sizeCalculator ?? throw new ArgumentNullException(nameof(sizeCalculator));

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}