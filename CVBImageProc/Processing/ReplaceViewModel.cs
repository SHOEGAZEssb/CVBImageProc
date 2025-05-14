using CVBImageProc.Processing.ValueProvider;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Replace"/> processor.
	/// </summary>
	internal sealed class ReplaceViewModel : FullProcessorViewModelBase
	{
		#region Properties

		/// <summary>
		/// ViewModel for the processors value provider.
		/// </summary>
		public ValueProviderViewModel<byte> ValueProviderVM { get; }

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly Replace _processor;

		#endregion Member

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="isActive">Startup IsActive state.</param>
		public ReplaceViewModel(Replace processor, bool isActive)
		  : base(processor, isActive)
		{
			_processor = processor;
			ValueProviderVM = new ValueProviderViewModel<byte>(_processor.ValueProvider);
			ValueProviderVM.SettingsChanged += SubVM_SettingsChanged;
		}

		/// <summary>
		/// Fires the SettingsChanged event when the
		/// pixel filter settings changed.
		/// </summary>
		/// <param name="sender">Ignored.</param>
		/// <param name="e">Ignored.</param>
		private void SubVM_SettingsChanged(object sender, EventArgs e)
		{
			OnSettingsChanged();
		}
	}
}