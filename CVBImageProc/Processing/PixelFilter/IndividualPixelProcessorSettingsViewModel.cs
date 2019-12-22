using CVBImageProc.MVVM;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="PixelFilterChain"/>.
  /// </summary>
  class IndividualPixelProcessorSettingsViewModel : ViewModelBase, IHasSettings
  {
    #region IHasSettings Implementation

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region Commands

    /// <summary>
    /// Command for adding a new pixel filter.
    /// </summary>
    public ICommand AddPixelFilterCommand { get; }

    /// <summary>
    /// Command for removing the selected pixel filter.
    /// </summary>
    public ICommand RemoveSelectedPixelFilterCommand { get; }

    #endregion Commands

    #region Properties

    /// <summary>
    /// Logic used when checking.
    /// </summary>
    public LogicMode Mode
    {
      get => _processor.PixelFilter.Mode;
      set
      {
        if(Mode != value)
        {
          _processor.PixelFilter.Mode = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// The configured filters.
    /// </summary>
    public ObservableCollection<IPixelFilterViewModel> Filters { get; }

    /// <summary>
    /// Currently selected filter.
    /// </summary>
    public IPixelFilterViewModel SelectedFilter
    {
      get => _selectedFilter;
      set
      {
        if (SelectedFilter != value)
        {
          _selectedFilter = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private IPixelFilterViewModel _selectedFilter;

    /// <summary>
    /// List of available filter types.
    /// </summary>
    public static IEnumerable<TypeViewModel> AvailableFilter { get; }

    /// <summary>
    /// Currently selected filter type.
    /// </summary>
    public TypeViewModel SelectedFilterType
    {
      get => _selectedFilterType;
      set
      {
        if (SelectedFilterType != value)
        {
          _selectedFilterType = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private TypeViewModel _selectedFilterType;

    #region AOI

    /// <summary>
    /// Indicates if an AOI should be used
    /// </summary>
    public bool UseAOI
    {
      get => _processor.UseAOI;
      set
      {
        if(UseAOI != value)
        {
          _processor.UseAOI = value;
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// X-Coordinate of the top left AOI corner.
    /// </summary>
    public int AOIX
    {
      get => _processor.AOI.Location.X;
      set
      {
        if(AOIX != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(new Point2D(value, aoi.Location.Y), aoi.Size);
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Y-Coordinate of the top left AOI corner.
    /// </summary>
    public int AOIY
    {
      get => _processor.AOI.Location.Y;
      set
      {
        if (AOIY != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(new Point2D(aoi.Location.X, value), aoi.Size);
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Width of the AOI.
    /// </summary>
    public int AOIWidth
    {
      get => _processor.AOI.Size.Width;
      set
      {
        if (AOIWidth != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(aoi.Location, new Size2D(value, aoi.Size.Height));
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    /// <summary>
    /// Height of the AOI.
    /// </summary>
    public int AOIHeight
    {
      get => _processor.AOI.Size.Height;
      set
      {
        if (AOIHeight != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(aoi.Location, new Size2D(aoi.Size.Width, value));
          NotifyOfPropertyChange();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }

    #endregion AOI

    #endregion Properties

    #region Member

    /// <summary>
    /// The filter chain.
    /// </summary>
    private readonly ICanProcessIndividualPixel _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Static constructor.
    /// </summary>
    static IndividualPixelProcessorSettingsViewModel()
    {
      AvailableFilter = Assembly.GetExecutingAssembly().GetTypes()
        .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPixelFilter)) && !mytype.IsAbstract).Select(i => new TypeViewModel(i)).ToArray();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public IndividualPixelProcessorSettingsViewModel(ICanProcessIndividualPixel processor)
    {
      _processor = processor ?? throw new ArgumentNullException(nameof(processor));
      AddPixelFilterCommand = new DelegateCommand((o) => AddPixelFilter());
      RemoveSelectedPixelFilterCommand = new DelegateCommand((o) => RemoveSelectedPixelFilter());

      SelectedFilterType = AvailableFilter.FirstOrDefault();
      Filters = new ObservableCollection<IPixelFilterViewModel>();
      Filters.CollectionChanged += Filters_CollectionChanged;

      foreach (var filter in _processor.PixelFilter.Filters)
        Filters.Add(CreatePixelFilterViewModel(filter));
    }

    #endregion Construction

    /// <summary>
    /// Adds a new pixel filter.
    /// </summary>
    private void AddPixelFilter()
    {
      if (SelectedFilterType == null)
        return;

      // add to model
      _processor.PixelFilter.Filters.Add((IPixelFilter)SelectedFilterType.Instanciate());

      // add to vm
      Filters.Add(CreatePixelFilterViewModel(_processor.PixelFilter.Filters.Last()));
      SelectedFilter = Filters.Last();
    }

    /// <summary>
    /// Creates a ViewModel for the given <paramref name="filter"/>.
    /// </summary>
    /// <param name="filter">Filter to create ViewModel for.</param>
    /// <returns>ViewModel for the <paramref name="filter"/>.</returns>
    private IPixelFilterViewModel CreatePixelFilterViewModel(IPixelFilter filter)
    {
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      switch (filter)
      {
        default:
          return new PixelFilterViewModel(filter);
      }
    }

    /// <summary>
    /// Removes the <see cref="SelectedFilter"/>.
    /// </summary>
    private void RemoveSelectedPixelFilter()
    {
      if (SelectedFilter == null)
        return;

      // remove from model
      int index = Filters.IndexOf(SelectedFilter);
      _processor.PixelFilter.Filters.RemoveAt(index);

      // remove from vm
      Filters.RemoveAt(index);
    }

    /// <summary>
    /// Links / unlinks events when the <see cref="Filters"/> changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Contains the event data.</param>
    private void Filters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (var filter in e.NewItems.OfType<IHasSettings>())
          filter.SettingsChanged += Filter_SettingsChanged;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var filter in e.OldItems.OfType<IHasSettings>())
          filter.SettingsChanged -= Filter_SettingsChanged;
      }

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Fires the <see cref="SettingsChanged"/> event
    /// when the settings of a filter changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void Filter_SettingsChanged(object sender, EventArgs e)
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}