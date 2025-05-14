using CVBImageProcLib.Processing.Filter;
using System;

namespace CVBImageProc.Processing.Filter
{
	/// <summary>
	/// ViewModel for the custom settings of
	/// the <see cref="Median"/> filter processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	internal sealed class MedianSettingsViewModel(Median processor) : SettingsViewModelBase
	{
		#region Properties

		/// <summary>
		/// Indicates if the higher median value
		/// should be used instead of the lower one.
		/// </summary>
		public bool UseHigherMedian
		{
			get => _processor.UseHigherMedian;
			set
			{
				if (UseHigherMedian != value)
				{
					_processor.UseHigherMedian = value;
					OnSettingsChanged();
					NotifyOfPropertyChange();
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly Median _processor = processor ?? throw new ArgumentNullException(nameof(processor));

		#endregion Member
		#region Construction

		#endregion Construction
	}
}