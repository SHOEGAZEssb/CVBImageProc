using Stemmer.Cvb;

namespace CVBImageProcLib.Processing.SizeCalculator
{
	/// <summary>
	/// Interface for an object that calculates
	/// a new image size.
	/// </summary>
	public interface ISizeCalculator
	{
		/// <summary>
		/// Gets the calculated size for
		/// the given <paramref name="img"/>.
		/// </summary>
		/// <param name="img">Img to calculate size for.</param>
		/// <returns>Calculated size.</returns>
		Size2D GetCalculatedSize(Image img);
	}
}