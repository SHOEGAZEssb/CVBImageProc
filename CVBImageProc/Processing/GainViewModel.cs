using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stemmer.Cvb;

namespace CVBImageProc.Processing
{
  class GainViewModel : ProcessorViewModel, IHasSettings, INeedImageInfo
  {
    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region INeedImageInfo Implementation

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

    public ObservableCollection<PlaneGainViewModel> Gains { get; }

    #endregion Properties

    #region Member

    private readonly Gain _processor;

    #endregion Member

    #region Construction

    public GainViewModel(Gain processor)
      : base(processor)
    {
      _processor = processor;
      Gains = new ObservableCollection<PlaneGainViewModel>();
      Gains.CollectionChanged += Gains_CollectionChanged;
    }

    #endregion Construction

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

    private void PlaneGainViewModel_SettingsChanged(object sender, EventArgs e)
    {
      var pgvm = sender as PlaneGainViewModel;
      _processor.Gains[pgvm.PlaneIndex] = pgvm.Value;

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
