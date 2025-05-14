using CVBImageProcLib.Processing.Filter;
using System;

namespace CVBImageProc.Processing.Filter
{
	/// <summary>
	/// ViewModel for the custom settings of
	/// the <see cref="OilPainting"/> filter processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	internal sealed class OilPaintingSettingsViewModel(OilPainting processor) : SettingsViewModelBase
	{
		#region Properties

		/// <summary>
		/// The number of intensity levels
		/// the result image will have.
		/// </summary>
		public int NumIntensityLevels
		{
			get => _processor.NumIntensityLevels;
			set
			{
				if (NumIntensityLevels != value)
				{
					_processor.NumIntensityLevels = value;
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
		private readonly OilPainting _processor = processor ?? throw new ArgumentNullException(nameof(processor));

		#endregion Member
		#region Construction

		#endregion Construction
	}
}