using System.ComponentModel;

namespace CVBImageProcLib.Processing.Filter
{
	/// <summary>
	/// Kernel size for filter processor.
	/// </summary>
	public enum KernelSize
	{
		/// <summary>
		/// 3x3 kernel.
		/// </summary>
		[Description("3x3")]
		ThreeByThree,

		/// <summary>
		/// 5x5 kernel.
		/// </summary>
		[Description("5x5")]
		FiveByFive,

		/// <summary>
		/// 7x7 kernel.
		/// </summary>
		[Description("7x7")]
		SevenBySeven
	}

	/// <summary>
	/// Interface for a filter processor.
	/// </summary>
	public interface IFilter : IProcessor
	{
		/// <summary>
		/// Kernel size to use.
		/// </summary>
		KernelSize KernelSize { get; set; }
	}
}