using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CVBImageProc.MVVM
{
  /// <summary>
  /// Base class for all ViewModels.
  /// </summary>
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged Implementation

    /// <summary>
    /// Event that is fired when a property changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion INotifyPropertyChanged Implementation

    /// <summary>
    /// Fires the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    protected void NotifyOfPropertyChange([CallerMemberName] string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}