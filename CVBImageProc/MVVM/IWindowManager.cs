namespace CVBImageProc.MVVM
{
	/// <summary>
	/// Interface for an object showing windows and dialogs.
	/// </summary>
	internal interface IWindowManager
	{
		/// <summary>
		/// Shows a modal dialog with the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">Context to use for determining the view.</param>
		/// <returns>Dialog result.</returns>
		bool? ShowDialog(object context);

		/// <summary>
		/// Shows a window with the given <paramref name="context"/>.
		/// </summary>
		/// <param name="context">Context to use for determining the view.</param>
		void ShowWindow(object context);
	}
}