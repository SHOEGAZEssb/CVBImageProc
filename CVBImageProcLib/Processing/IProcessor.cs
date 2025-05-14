using Stemmer.Cvb;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Interface for an image processor.
	/// </summary>
	public interface IProcessor
	{
		/// <summary>
		/// Name of the processor.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Processes the <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage">Image to process.</param>
		/// <returns>Processed image.</returns>
		Image Process(Image inputImage);
	}
}