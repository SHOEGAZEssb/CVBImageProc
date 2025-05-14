using CVBImageProcLib.Processing.PixelFilter;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Grouping interface for a processor
	/// with full configuration capabilities.
	/// </summary>
	public interface IFullProcessor : IAOIPlaneProcessor, ICanProcessIndividualPixel
	{ }
}