using Stemmer.Cvb;

namespace CVBImageProcLib.Processing.PixelFilter
{
	/// <summary>
	/// Interface for a processor that can
	/// process individual portions
	/// of an image.
	/// </summary>
	public interface ICanProcessIndividualRegions
	{
		/// <summary>
		/// If true, uses the <see cref="AOI"/>
		/// while processing.
		/// </summary>
		bool UseAOI { get; set; }

		/// <summary>
		/// The AOI to process.
		/// </summary>
		Rect AOI { get; set; }
	}
}
