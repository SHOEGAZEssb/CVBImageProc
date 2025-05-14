using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="MonoToMultiplane"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Startup IsActive state.</param>
	internal sealed class MonoToMultiplaneViewModel(MonoToMultiplane processor, bool isActive) : ProcessorViewModel(processor, isActive), IHasSettings
	{
		#region Properties

		/// <summary>
		/// Event that is fired when one of
		/// the settings changed.
		/// </summary>
		public event EventHandler SettingsChanged;

		/// <summary>
		/// Amount of planes in the new image.
		/// </summary>
		public int NumPlanes
		{
			get => _processor.NumPlanes;
			set
			{
				if (NumPlanes != value)
				{
					_processor.NumPlanes = value;
					NotifyOfPropertyChange();
					SettingsChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly MonoToMultiplane _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}
