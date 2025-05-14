using Stemmer.Cvb;
using System;

namespace CVBImageProc.ImageSource
{
	/// <summary>
	/// Image source providing a non-changing image.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="img">The image to provide.</param>
	/// <exception cref="ArgumentNullException">When <paramref name="img"/> is null.</exception>
	internal sealed class StaticImageSource(Image img) : IImageSource
	{
		#region Properties

		/// <summary>
		/// The image to provide.
		/// </summary>
		public Image CurrentImage { get; } = img ?? throw new ArgumentNullException(nameof(img));

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}
