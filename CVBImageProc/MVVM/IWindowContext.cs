using System;

namespace CVBImageProc.MVVM
{
	/// <summary>
	/// Interface for an object that has a dialog result.
	/// </summary>
	internal interface IWindowContext
	{
		/// <summary>
		/// The result of the dialog.
		/// </summary>
		bool? Result { get; }

		/// <summary>
		/// Event that is fired when the view should be closed.
		/// </summary>
		event EventHandler CloseRequested;
	}
}