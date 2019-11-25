using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Stemmer.Cvb;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Gain"/> processor.
  /// </summary>
  class GainViewModel : ProcessorViewModel, IHasSettings, INeedImageInfo
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
      foreach (var pgvm in Gains.ToArray())
        Gains.Remove(pgvm);

      var newGains = img.Planes.Select(p => new PlaneGainViewModel(p.Plane)).ToArray();

      // take old values if length is the same
      if (_processor.Gains != null && newGains.Length == _processor.Gains.Length)
      {
        for (int i = 0; i < newGains.Length; i++)
          newGains[i].Value = (int)_processor.Gains[i];
      }

      _processor.Gains = newGains.Select(i => (double)i.Value).ToArray();

      foreach (var pgvm in newGains)
        Gains.Add(pgvm);

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion INeedImageInfo Implementation

    #region Properties

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    public bool WrapAround
    {
      get => _processor.WrapAround;
      set
      {
        if (WrapAround != value)
        {
          _processor.WrapAround = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// List containing the gain per plane.
    /// </summary>
    public ObservableCollection<PlaneGainViewModel> Gains { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The gain processor.
    /// </summary>
    private readonly Gain _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The gain processor.</param>
    public GainViewModel(Gain processor)
      : base(processor)
    {
      _processor = processor;
      Gains = new ObservableCollection<PlaneGainViewModel>();
      Gains.CollectionChanged += Gains_CollectionChanged;
    }

    #endregion Construction

    /// <summary>
    /// Links / unlinks events when the <see cref="Gains"/> changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Contains the event data.</param>
    private void Gains_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (var pgvm in e.NewItems.OfType<PlaneGainViewModel>())
          pgvm.SettingsChanged += PlaneGainViewModel_SettingsChanged;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var pgvm in e.OldItems.OfType<PlaneGainViewModel>())
          pgvm.SettingsChanged -= PlaneGainViewModel_SettingsChanged;
      }
    }

    /// <summary>
    /// Fires the <see cref="SettingsChanged"/> event when the
    /// setting of a <see cref="Gains"/> entry changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void PlaneGainViewModel_SettingsChanged(object sender, EventArgs e)
    {
      var pgvm = sender as PlaneGainViewModel;
      _processor.Gains[pgvm.PlaneIndex] = pgvm.Value;

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}