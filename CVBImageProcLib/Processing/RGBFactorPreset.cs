namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Preset for the <see cref="RGBFactors"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="factorRR">Factor of the R component of the R pixel.</param>
	/// <param name="factorRG">Factor of the G component of the R pixel.</param>
	/// <param name="factorRB">Factor of the B component of the R pixel.</param>
	/// <param name="factorGR">Factor of the R component of the G pixel.</param>
	/// <param name="factorGG">Factor of the G component of the G pixel.</param>
	/// <param name="factorGB">Factor of the B component of the G pixel.</param>
	/// <param name="factorBR">Factor of the R component of the B pixel.</param>
	/// <param name="factorBG">Factor of the G component of the B pixel.</param>
	/// <param name="factorBB">Factor of the B component of the B pixel.</param>
	public readonly struct RGBFactorPreset(double factorRR, double factorRG, double factorRB,
												 double factorGR, double factorGG, double factorGB,
												 double factorBR, double factorBG, double factorBB)
	{
		#region Properties

		/// <summary>
		/// Factor of the R component of the R pixel.
		/// </summary>
		public double FactorRR { get; } = factorRR;

		/// <summary>
		/// Factor of the G component of the R pixel.
		/// </summary>
		public double FactorRG { get; } = factorRG;

		/// <summary>
		/// Factor of the B component of the R pixel.
		/// </summary>
		public double FactorRB { get; } = factorRB;

		/// <summary>
		/// Factor of the R component of the G pixel.
		/// </summary>
		public double FactorGR { get; } = factorGR;

		/// <summary>
		/// Factor of the G component of the G pixel.
		/// </summary>
		public double FactorGG { get; } = factorGG;

		/// <summary>
		/// Factor of the B component of the G pixel.
		/// </summary>
		public double FactorGB { get; } = factorGB;

		/// <summary>
		/// Factor of the R component of the B pixel.
		/// </summary>
		public double FactorBR { get; } = factorBR;

		/// <summary>
		/// Factor of the G component of the B pixel.
		/// </summary>
		public double FactorBG { get; } = factorBG;

		/// <summary>
		/// Factor of the B component of the B pixel.
		/// </summary>
		public double FactorBB { get; } = factorBB;

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}