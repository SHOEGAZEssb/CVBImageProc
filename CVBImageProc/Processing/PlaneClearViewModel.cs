using Stemmer.Cvb;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="PlaneClear"/> processor.
  /// </summary>
  class PlaneClearViewModel : ProcessorViewModel, IHasSettings, INeedImageInfo
  {
    #region IHasSettings Implementation

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region INeedImageInfo Implementation

    /// <summary>
    /// Updates the image information.
    /// </summary>
    /// <param name="img">Image to pull info from.</param>
    public void UpdateImageInfo(Image img)
    {
      // remove old info
      // remove individually because we need to unlink events
      foreach (var csvm in Clears.ToArray())
        Clears.Remove(csvm);

      var newClears = img.Planes.Select(p => new PlaneClearStateViewModel(p.Plane)).ToArray();

      // take old values if length is the same
      if (_processor.Clears != null && newClears.Length == _processor.Clears.Length)
      {
        for (int i = 0; i < newClears.Length; i++)
          newClears[i].Clear = _processor.Clears[i];
      }

      _processor.Clears = newClears.Select(i => i.Clear).ToArray();

      foreach (var csvm in newClears)
        Clears.Add(csvm);

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion INeedImageInfo Implementation

    #region Properties

    /// <summary>
    /// Individual plane clear states.
    /// </summary>
    public ObservableCollection<PlaneClearStateViewModel> Clears { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly PlaneClear _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public PlaneClearViewModel(PlaneClear processor)
      : base(processor)
    {
      _processor = processor;
      Clears = new ObservableCollection<PlaneClearStateViewModel>();
      Clears.CollectionChanged += Clears_CollectionChanged;
    }

    #endregion Construction

    /// <summary>
    /// Links / unlinks events when the <see cref="Clears"/> changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Contains the event data.</param>
    private void Clears_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (var csvm in e.NewItems.OfType<PlaneClearStateViewModel>())
          csvm.SettingsChanged += PlaneClearStateViewModel_SettingsChanged;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var csvm in e.OldItems.OfType<PlaneClearStateViewModel>())
          csvm.SettingsChanged -= PlaneClearStateViewModel_SettingsChanged;
      }
    }

    /// <summary>
    /// Fires the <see cref="SettingsChanged"/> event when the
    /// setting of a <see cref="Clears"/> entry changed.
    /// </summary>
    /// <param name="sender"><see cref="PlaneClearStateViewModel"/> whose settings changed.</param>
    /// <param name="e">Ignored.</param>
    private void PlaneClearStateViewModel_SettingsChanged(object sender, EventArgs e)
    {
      var csvm = sender as PlaneClearStateViewModel;
      _processor.Clears[csvm.PlaneIndex] = csvm.Clear;

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}