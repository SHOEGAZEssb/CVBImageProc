using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
	/// <summary>
	/// ViewModel for the <see cref="Randomize"/> pixel filter.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="filter">The filter.</param>
	/// <param name="isActive">Active state of the filter.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
	internal sealed class RandomizeViewModel(Randomize filter, bool isActive) : PixelFilterViewModelBase(filter, isActive), IPixelAutoFilterViewModel
	{
		#region Properties

		/// <summary>
		/// Chance that the check passes.
		/// </summary>
		public double Chance
		{
			get => _filter.Chance;
			set
			{
				if (Chance != value)
				{
					_filter.Chance = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The filter.
		/// </summary>
		private readonly Randomize _filter = filter;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}