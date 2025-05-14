using CVBImageProcLib.Processing.PixelFilter;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
	/// <summary>
	/// ViewModel for a <see cref="IPixelValueFilter"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="filter">The filter.</param>
	/// <param name="isActive">Active state of the filter.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="filter"/> is null.</exception>
	internal sealed class PixelValueFilterViewModel(IPixelValueFilter filter, bool isActive) : PixelFilterViewModelBase(filter, isActive)
	{
		#region Properties

		/// <summary>
		/// Byte to compare to.
		/// </summary>
		public byte CompareByte
		{
			get => _filter.CompareByte;
			set
			{
				if (CompareByte != value)
				{
					_filter.CompareByte = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		/// <summary>
		/// Max value of the <see cref="CompareByte"/>.
		/// </summary>
		public byte MaxCompareByte => _filter.MaxCompareByte;

		/// <summary>
		/// Min value of the <see cref="CompareByte"/>.
		/// </summary>
		public byte MinCompareByte => _filter.MinCompareByte;

		#endregion Properties

		#region Member

		/// <summary>
		/// The filter.
		/// </summary>
		private readonly IPixelValueFilter _filter = filter;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}