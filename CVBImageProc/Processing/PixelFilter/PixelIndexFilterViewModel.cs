using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
	/// <summary>
	/// ViewModel for a <see cref="IPixelIndexFilter"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="filter">The filter.</param>
	/// <param name="isActive">Active state of the filter.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
	internal sealed class PixelIndexFilterViewModel(IPixelIndexFilter filter, bool isActive) : PixelFilterViewModelBase(filter, isActive)
	{
		#region Properties

		/// <summary>
		/// Index value to compare to.
		/// </summary>
		public int CompareValue
		{
			get => _filter.CompareValue;
			set
			{
				if (CompareValue != value)
				{
					_filter.CompareValue = value;
					OnSettingsChanged();
					NotifyOfPropertyChange();
				}
			}
		}

		/// <summary>
		/// Minimum index value to compare to.
		/// </summary>
		public int MinCompareValue => _filter.MinCompareValue;

		#endregion Properties

		#region Member

		/// <summary>
		/// The filter.
		/// </summary>
		private readonly IPixelIndexFilter _filter = filter;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}