using System;
using System.Windows;

namespace CVBImageProc.MVVM
{
  /// <summary>
  /// Interaction logic for ContextWindow.xaml
  /// </summary>
  public partial class ContextWindow : Window
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context">Context object for the ContentControl.</param>
    public ContextWindow(object context)
    {
      InitializeComponent();
      Content.Content = context ?? throw new ArgumentNullException(nameof(context));
      if(context is IWindowContext c)
        c.CloseRequested += ContextCloseRequested;
    }

    private void ContextCloseRequested(object sender, EventArgs e)
    {
      var context = Content.Content as IWindowContext;
      context.CloseRequested -= ContextCloseRequested;
      DialogResult = context.Result;
      Close();
    }
  }
}