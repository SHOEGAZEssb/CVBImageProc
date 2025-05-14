namespace CVBImageProc.Processing.Filter
{
	/// <summary>
	/// ViewModel for a value of a filter kernel.
	/// </summary>
	internal sealed class KernelValueViewModel : SettingsViewModelBase
	{
		/// <summary>
		/// The value.
		/// </summary>
		public int Value
		{
			get => _value;
			set
			{
				if (Value != value)
				{
					_value = value;
					OnSettingsChanged();
					NotifyOfPropertyChange();
				}
			}
		}
		private int _value;

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="value">Initial value.</param>
		public KernelValueViewModel(int value)
		{
			Value = value;
		}

		#endregion Construction
	}
}