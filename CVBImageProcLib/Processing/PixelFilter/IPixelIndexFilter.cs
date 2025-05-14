namespace CVBImageProcLib.Processing.PixelFilter
{
	/// <summary>
	/// Interface for a pixel filter checking indexes.
	/// </summary>
	public interface IPixelIndexFilter : IPixelFilter
	{
		/// <summary>
		/// Index value to compare to.
		/// </summary>
		int CompareValue { get; set; }

		/// <summary>
		/// Minimum index value to compare to.
		/// </summary>
		int MinCompareValue { get; }

		/// <summary>
		/// Checks if the given <paramref name="index"/> passes the filter.
		/// </summary>
		/// <param name="index">Index to check.</param>
		/// <returns>True if the <paramref name="index"/> passes the filter,
		/// otherwise false.</returns>
		bool Check(int index);
	}
}