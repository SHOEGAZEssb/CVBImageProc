using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.PixelFilter;
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
  class PixelFilterChainViewModel : SettingsViewModelBase
  {
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
        if (Mode != value)
        {
          _processor.PixelFilter.Mode = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
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
    static PixelFilterChainViewModel()
    {
      AvailableFilter = Assembly.GetAssembly(typeof(IPixelFilter)).GetTypes()
        .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPixelFilter)) && !mytype.IsAbstract).Select(i => new TypeViewModel(i))
        .OrderBy(t => t.Name).ToArray();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public PixelFilterChainViewModel(ICanProcessIndividualPixel processor)
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
      var filter = (IPixelFilter)SelectedFilterType.Instanciate();
      if (filter is IPixelValueFilter vf)
        _processor.PixelFilter.ValueFilters.Add(new KeyValuePair<IPixelValueFilter, bool>(vf, true));
      else if (filter is IPixelIndexFilter inf)
        _processor.PixelFilter.IndexFilters.Add(new KeyValuePair<IPixelIndexFilter, bool>(inf, true));
      else if (filter is IPixelAutoFilter auto)
        _processor.PixelFilter.AutoFilters.Add(new KeyValuePair<IPixelAutoFilter, bool>(auto, true));
      else
        return;

      // add to vm
      Filters.Add(CreatePixelFilterViewModel(new KeyValuePair<IPixelFilter, bool>(filter, true)));
      SelectedFilter = Filters.Last();
    }

    /// <summary>
    /// Creates a ViewModel for the given <paramref name="kvp"/>.
    /// </summary>
    /// <param name="kvp">Filter to create ViewModel for.</param>
    /// <returns>ViewModel for the <paramref name="kvp"/>.</returns>
    private static IPixelFilterViewModel CreatePixelFilterViewModel(KeyValuePair<IPixelFilter, bool> kvp)
    {
      if (kvp.Key == null)
        throw new ArgumentNullException(nameof(kvp));

      switch (kvp.Key)
      {
        case IPixelValueFilter vf:
          return new PixelValueFilterViewModel(vf, kvp.Value);
        case IPixelIndexFilter inf:
          return new PixelIndexFilterViewModel(inf, kvp.Value);
        case Randomize r:
          return new RandomizeViewModel(r, kvp.Value);
        default:
          throw new ArgumentException("Unknown filter type", nameof(kvp));
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
      var pfc = _processor.PixelFilter;
      if (SelectedFilter is PixelValueFilterViewModel pvf)
      {
        int index = pfc.ValueFilters.IndexOf(pfc.ValueFilters.First(kvp => kvp.Key == pvf.Filter));
        pfc.ValueFilters.RemoveAt(index);
      }
      else if (SelectedFilter is PixelIndexFilterViewModel pif)
      {
        int index = pfc.IndexFilters.IndexOf(pfc.IndexFilters.First(kvp => kvp.Key == pif.Filter));
        pfc.IndexFilters.RemoveAt(index);
      }
      else if (SelectedFilter is IPixelAutoFilterViewModel paf)
      {
        int index = pfc.AutoFilters.IndexOf(pfc.AutoFilters.First(kvp => kvp.Key == paf.Filter));
        pfc.AutoFilters.RemoveAt(index);
      }
      else
        return;

      // remove from vm
      Filters.Remove(SelectedFilter);
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
        foreach (var filter in e.NewItems.OfType<IPixelFilterViewModel>())
        {
          filter.SettingsChanged += Filter_SettingsChanged;
          filter.IsActiveChanged += Filter_IsActiveChanged;
        }
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var filter in e.OldItems.OfType<IPixelFilterViewModel>())
        {
          filter.SettingsChanged -= Filter_SettingsChanged;
          filter.IsActiveChanged -= Filter_IsActiveChanged;
        }
      }

      OnSettingsChanged();
    }

    /// <summary>
    /// Fires the SettingsChanged event
    /// when the settings of a filter changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void Filter_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }

    /// <summary>
    /// Updates the filter model when a vm
    /// IsActive state changes.
    /// </summary>
    /// <param name="sender">The VM that triggered the event.</param>
    /// <param name="e">Ignored.</param>
    private void Filter_IsActiveChanged(object sender, EventArgs e)
    {
      if (!(sender is IPixelFilterViewModel vm))
        return;

      var pfc = _processor.PixelFilter;
      if (vm is PixelValueFilterViewModel pvf)
      {
        int index = pfc.ValueFilters.IndexOf(pfc.ValueFilters.First(kvp => kvp.Key == pvf.Filter));
        pfc.ValueFilters[index] = new KeyValuePair<IPixelValueFilter, bool>(pvf.Filter, pvf.IsActive);
      }
      else if (vm is PixelIndexFilterViewModel pif)
      {
        int index = pfc.IndexFilters.IndexOf(pfc.IndexFilters.First(kvp => kvp.Key == pif.Filter));
        pfc.IndexFilters[index] = new KeyValuePair<IPixelIndexFilter, bool>(pif.Filter, pif.IsActive);
      }
      else if (vm is IPixelAutoFilterViewModel paf)
      {
        int index = pfc.AutoFilters.IndexOf(pfc.AutoFilters.First(kvp => kvp.Key == paf.Filter));
        pfc.AutoFilters[index] = new KeyValuePair<IPixelAutoFilter, bool>(paf.Filter, paf.IsActive);
      }
    }
  }
}