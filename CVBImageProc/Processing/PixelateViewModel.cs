using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Pixelate"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Default active state.</param>
	internal sealed class PixelateViewModel(Pixelate processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
		#region Properties

		/// <summary>
		/// Size of each new "pixel".
		/// </summary>
		public int PixelateSize
		{
			get => _processor.PixelateSize;
			set
			{
				if (PixelateSize != value)
				{
					_processor.PixelateSize = value;
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
		private readonly Pixelate _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}