using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
{
	/// <summary>
	/// Pixel filter that checks if a given
	/// pixel index is equal the configured value.
	/// </summary>
	[DataContract]
	[DisplayName("Equals (Index)")]
	public sealed class EqualsIndex : PixelIndexFilterBase
	{
		/// <summary>
		/// Name of the filter.
		/// </summary>
		public override string Name => "Equals (Index)";

		/// <summary>
		/// Checks if the given <paramref name="index"/> passes the filter.
		/// </summary>
		/// <param name="index">Index to check.</param>
		/// <returns>True if the <paramref name="index"/> passes the filter,
		/// otherwise false.</returns>
		public override bool Check(int index)
		{
			return Invert ? index != CompareValue : index == CompareValue;
		}
	}
}
