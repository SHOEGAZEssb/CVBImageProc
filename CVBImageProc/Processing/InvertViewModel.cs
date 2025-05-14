using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Invert"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Startup IsActive state.</param>
	internal sealed class InvertViewModel(Invert processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
	}
}