using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Sort"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Startup IsActive state.</param>
	internal sealed class SortViewModel(Sort processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
		#region Properties

		/// <summary>
		/// Mode to use while sorting.
		/// </summary>
		public SortMode Mode
		{
			get => _processor.Mode;
			set
			{
				if (Mode != value)
				{
					_processor.Mode = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly Sort _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}