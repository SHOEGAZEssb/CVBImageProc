namespace CVBImageProc.MVVM
{
  class WindowManager : IWindowManager
  {
    public bool? ShowDialog(object context)
    {
      var win = new ContextWindow(context);
      return win.ShowDialog();
    }

    public void ShowWindow(object context)
    {
      var win = new ContextWindow(context);
      win.Show();
    }
  }
}