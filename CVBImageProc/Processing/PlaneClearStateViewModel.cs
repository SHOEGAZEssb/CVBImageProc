namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for an individual plane clear state
	/// in the <see cref="PlaneClearViewModel"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="planeIndex">Index of the plane in the parent image.</param>
	internal sealed class PlaneClearStateViewModel(int planeIndex) : PlaneSettingsViewModelBase(planeIndex)
	{
		#region Properties

		/// <summary>
		/// Clear state of this plane.
		/// </summary>
		public bool Clear
		{
			get => _clear;
			set
			{
				if (Clear != value)
				{
					_clear = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}
		private bool _clear;

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}