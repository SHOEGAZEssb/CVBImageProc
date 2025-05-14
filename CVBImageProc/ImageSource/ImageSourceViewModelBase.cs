using CVBImageProc.MVVM;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.ImageSource
{
	/// <summary>
	/// Base class for <see cref="IImageSource"/> ViewModels.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="imageSource">The image source.</param>
	public abstract class ImageSourceViewModelBase(IImageSource imageSource) : ViewModelBase, IDisposable
	{
		#region Properties

		/// <summary>
		/// The current image to provide.
		/// </summary>
		public Image CurrentImage => _imageSource.CurrentImage;

		#endregion Properties

		#region Member

		/// <summary>
		/// The image source.
		/// </summary>
		private readonly IImageSource _imageSource = imageSource ?? throw new ArgumentNullException(nameof(imageSource));

		#endregion Member
		#region Construction

		#endregion Construction

		#region IDisposable Implementation

		/// <summary>
		/// Gets if this object has been disposed.
		/// </summary>
		private bool _disposedValue;

		/// <summary>
		/// Disposes this object.
		/// </summary>
		/// <param name="disposing">True if called by the user.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					if (_imageSource is IDisposable d)
						d.Dispose();
				}

				_disposedValue = true;
			}
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~ImageSourceViewModelBase()
		{
			Dispose(disposing: false);
		}

		/// <summary>
		/// Disposes this object.
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Implementation
	}
}
