using CVBImageProc.MVVM;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVBImageProc.ImageSource
{
	/// <summary>
	/// ViewModel for a <see cref="VideoImageSource"/>.
	/// </summary>
	internal sealed class VideoImageSourceViewModel : ChangingImageSourceViewModelBase
	{
		#region Properties

		/// <summary>
		/// The grab state of the video.
		/// </summary>
		public bool Grab
		{
			get => _videoImageSource.Grab;
			set
			{
				if (Grab != value)
				{
					_videoImageSource.Grab = value;
					NotifyOfPropertyChange();
				}
			}
		}

		/// <summary>
		/// Command for toggling the <see cref="Grab"/>.
		/// </summary>
		public ICommand ToggleGrabCommand { get; }

		/// <summary>
		/// Command for snapping a single image.
		/// </summary>
		public ICommand SnapCommand { get; }

		#endregion Properties

		#region Member

		/// <summary>
		/// The image source.
		/// </summary>
		private readonly VideoImageSource _videoImageSource;

		#endregion Member

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="imageSource">The image source.</param>
		public VideoImageSourceViewModel(VideoImageSource imageSource)
		  : base(imageSource)
		{
			_videoImageSource = imageSource;
			ToggleGrabCommand = new DelegateCommand((o) => ToggleGrab());
			SnapCommand = new DelegateCommand(async (o) => await Snap().ConfigureAwait(false));
		}

		#endregion Construction

		/// <summary>
		/// Toggles the <see cref="Grab"/>.
		/// </summary>
		private void ToggleGrab()
		{
			Grab = !Grab;
		}

		/// <summary>
		/// Snaps a single image.
		/// </summary>
		/// <returns>Task.</returns>
		private async Task Snap()
		{
			await _videoImageSource.SnapAsync().ConfigureAwait(false);
		}
	}
}