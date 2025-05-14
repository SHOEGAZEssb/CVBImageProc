using Stemmer.Cvb;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// Interface for an object that needs
	/// information about the image it will work with.
	/// </summary>
	internal interface INeedImageInfo
	{
		/// <summary>
		/// Updates the image information.
		/// </summary>
		/// <param name="img">Image to pull info from.</param>
		void UpdateImageInfo(Image img);
	}
}