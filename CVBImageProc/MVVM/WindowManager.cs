namespace CVBImageProc.MVVM
{
  /// <summary>
  /// Object managing windows.
  /// </summary>
  internal class WindowManager : IWindowManager
  {
    /// <summary>
    /// Shows a modal dialog with the given <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Context to use for determining the view.</param>
    /// <returns>Dialog result.</returns>
    public bool? ShowDialog(object context)
    {
      var win = new ContextWindow(context);
      return win.ShowDialog();
    }

    /// <summary>
    /// Shows a window with the given <paramref name="context"/>.
    /// </summary>
    /// <param name="context">Context to use for determining the view.</param>
    public void ShowWindow(object context)
    {
      var win = new ContextWindow(context);
      win.Show();
    }
  }
}