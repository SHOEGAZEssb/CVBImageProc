using CVBImageProcLib.Processing.SizeCalculator;

namespace CVBImageProc.Processing.SizeCalculator
{
	/// <summary>
	/// ViewModel for the <see cref="PercentageSizeCalculator"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="sizeCalculator">The calculator.</param>
	internal sealed class PercentageSizeCalculatorViewModel(PercentageSizeCalculator sizeCalculator) : SizeCalculatorViewModelBase(sizeCalculator)
	{
		#region Properties

		/// <summary>
		/// The percentage to use of the input size.
		/// </summary>
		public double Percentage
		{
			get => _sizeCalculator.Percentage;
			set
			{
				if (Percentage != value)
				{
					_sizeCalculator.Percentage = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The size calculator.
		/// </summary>
		private readonly PercentageSizeCalculator _sizeCalculator = sizeCalculator;

		#endregion Member
	}
}