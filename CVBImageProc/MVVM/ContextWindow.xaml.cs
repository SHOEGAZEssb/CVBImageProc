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
    }
  }
}