namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// RGB-Pixel struct.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="r">The R pixel.</param>
	/// <param name="g">The G pixel.</param>
	/// <param name="b">The b pixel.</param>
	internal readonly struct RGBPixel(byte r, byte g, byte b)
	{
		#region Properties

		/// <summary>
		/// The R pixel.
		/// </summary>
		public byte R { get; } = r;

		/// <summary>
		/// The G pixel.
		/// </summary>
		public byte G { get; } = g;

		/// <summary>
		/// The B pixel.
		/// </summary>
		public byte B { get; } = b;

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}