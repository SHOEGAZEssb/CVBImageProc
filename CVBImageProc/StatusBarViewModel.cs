using CVBImageProc.MVVM;

namespace CVBImageProc
{
	/// <summary>
	/// ViewModel for the status bar.
	/// </summary>
	public sealed class StatusBarViewModel : ViewModelBase
	{
		#region Properties

		/// <summary>
		/// Current status message.
		/// </summary>
		public string StatusMessage
		{
			get => _statusMessage;
			set
			{
				if (StatusMessage != value)
				{
					_statusMessage = value;
					NotifyOfPropertyChange();
				}
			}
		}
		private string _statusMessage;

		#endregion Properties
	}
}