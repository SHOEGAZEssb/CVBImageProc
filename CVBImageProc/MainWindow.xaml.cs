using CVBImageProc.Properties;
using System.Windows;

namespace CVBImageProc
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml.
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Constructor.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Settings.Default.Save();
    }
  }
}