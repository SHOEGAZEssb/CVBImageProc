using CVBImageProc.MVVM;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for a generic <see cref="IProcessor"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The actual processor.</param>
	/// <param name="isActive">Startup IsActive state.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="processor"/> is null.</exception>
	internal class ProcessorViewModel(IProcessor processor, bool isActive) : ViewModelBase, IProcessorViewModel
	{
		#region IProcessorViewModel Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public string Name => Processor.Name;

		/// <summary>
		/// Gets if the last processing operation of
		/// this processor resulted in an error.
		/// </summary>
		public bool IsFaulted
		{
			get => _isFaulted;
			set
			{
				if (IsFaulted != value)
				{
					_isFaulted = value;
					NotifyOfPropertyChange();
				}
			}
		}
		private bool _isFaulted;

		/// <summary>
		/// Event that is fired when the
		/// <see cref="IsActive"/> changed.
		/// </summary>
		public event EventHandler IsActiveChanged;

		/// <summary>
		/// Indicates if this processor is
		/// active in the processor chain.
		/// </summary>
		public bool IsActive
		{
			get => _isActive;
			set
			{
				if (IsActive != value)
				{
					_isActive = value;
					IsActiveChanged?.Invoke(this, EventArgs.Empty);
					NotifyOfPropertyChange();
				}
			}
		}
		private bool _isActive = isActive;

		/// <summary>
		/// The wrapped processor.
		/// </summary>
		public IProcessor Processor { get; } = processor ?? throw new ArgumentNullException(nameof(processor));

		#endregion IProcessorViewModel Implementation
		#region Construction

		#endregion Construction
	}
}