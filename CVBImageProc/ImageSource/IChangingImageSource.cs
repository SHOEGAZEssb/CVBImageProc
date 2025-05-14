using System;

namespace CVBImageProc.ImageSource
{
	/// <summary>
	/// Interface for a image source whose image can change.
	/// </summary>
	public interface IChangingImageSource : IImageSource
	{
		/// <summary>
		/// Event that is fired when the <see cref="IImageSource.CurrentImage"/> changed.
		/// </summary>
		event EventHandler CurrentImageChanged;
	}
}
