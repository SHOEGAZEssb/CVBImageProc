namespace CVBImageProc.Processing
{
	/// <summary>
	/// Base ViewModel for individual plane settings of a processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="planeIndex">Index of the plane in the image.</param>
	internal abstract class PlaneSettingsViewModelBase(int planeIndex) : SettingsViewModelBase
	{
		#region Properties

		/// <summary>
		/// Index of the plane in the image.
		/// </summary>
		public int PlaneIndex { get; } = planeIndex;

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}