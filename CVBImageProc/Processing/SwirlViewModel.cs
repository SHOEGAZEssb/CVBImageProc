using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Swirl"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Default active state.</param>
	internal sealed class SwirlViewModel(Swirl processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
		#region Properties

		/// <summary>
		/// Factor by which to swirl.
		/// </summary>
		public double Factor
		{
			get => _processor.Factor;
			set
			{
				if (Factor != value)
				{
					_processor.Factor = value;
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
		private readonly Swirl _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}
