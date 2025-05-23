﻿namespace CVBImageProcLib.Processing.PixelFilter
{
	/// <summary>
	/// Interface for a pixel filter.
	/// </summary>
	public interface IPixelFilter
	{
		/// <summary>
		/// Name of the filter.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// If true, inverts the logic of the filter.
		/// </summary>
		bool Invert { get; set; }
	}
}