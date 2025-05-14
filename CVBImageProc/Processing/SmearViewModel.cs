using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Smear"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Default active state.</param>
	internal sealed class SmearViewModel(Smear processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
		#region Properties

		/// <summary>
		/// Direction to smear pixels in.
		/// </summary>
		public SmearMode Mode
		{
			get => _processor.Mode;
			set
			{
				if (Mode != value)
				{
					_processor.Mode = value;
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
		private readonly Smear _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}