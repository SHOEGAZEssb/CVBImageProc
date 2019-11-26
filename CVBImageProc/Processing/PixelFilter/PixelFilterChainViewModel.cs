using CVBImageProc.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVBImageProc.Processing.PixelFilter
{
  class PixelFilterChainViewModel : ViewModelBase, IHasSettings
  {
    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    #endregion IHasSettings Implementation

    #region Commands

    public ICommand AddPixelFilterCommand { get; }

    public ICommand RemoveSelectedPixelFilterCommand { get; }

    #endregion Commands

    #region Properties

    public ObservableCollection<IPixelFilterViewModel> Filters { get; }

    public IPixelFilterViewModel SelectedFilter
    {
      get => _selectedFilter;
      set
      {
        if(SelectedFilter != value)
        {
          _selectedFilter = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private IPixelFilterViewModel _selectedFilter;

    public IEnumerable<TypeViewModel> AvailableFilter { get; }

    public TypeViewModel SelectedFilterType
    {
      get => _selectedFilterType;
      set
      {
        if(SelectedFilterType != value)
        {
          _selectedFilterType = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private TypeViewModel _selectedFilterType;

    #endregion Properties

    #region Member

    private readonly PixelFilterChain _filterChain;

    #endregion Member

    #region Construction

    public PixelFilterChainViewModel(PixelFilterChain filterChain)
    {
      _filterChain = filterChain ?? throw new ArgumentNullException(nameof(filterChain));
      AddPixelFilterCommand = new DelegateCommand((o) => AddPixelFilter());
      RemoveSelectedPixelFilterCommand = new DelegateCommand((o) => RemoveSelectedPixelFilter());

      AvailableFilter = Assembly.GetExecutingAssembly().GetTypes()
           .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPixelFilter))).Select(i => new TypeViewModel(i)).ToArray();
      SelectedFilterType = AvailableFilter.FirstOrDefault();

      Filters = new ObservableCollection<IPixelFilterViewModel>();
      Filters.CollectionChanged += Filters_CollectionChanged;

      foreach (var filter in _filterChain.Filters)
        Filters.Add(CreatePixelFilterViewModel(filter));
    }

    #endregion Construction

    private void AddPixelFilter()
    {
      if (SelectedFilterType == null)
        return;

      // add to model
      _filterChain.Filters.Add((IPixelFilter)SelectedFilterType.Instanciate());

      // add to vm
      Filters.Add(CreatePixelFilterViewModel(_filterChain.Filters.Last()));
    }

    private IPixelFilterViewModel CreatePixelFilterViewModel(IPixelFilter filter)
    {
      if (filter == null)
        throw new ArgumentNullException(nameof(filter));

      switch(filter)
      {
        case Equals e:
          return new EqualsViewModel(e);
        default:
          throw new ArgumentException($"Unknown pixel filter type: {filter.GetType()}", nameof(filter));
      }
    }

    private void RemoveSelectedPixelFilter()
    {
      if (SelectedFilter == null)
        return;

      // remove from model
      int index = Filters.IndexOf(SelectedFilter);
      _filterChain.Filters.RemoveAt(index);

      // remove from vm
      Filters.RemoveAt(index);
    }

    private void Filters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if(e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach(var filter in e.NewItems.OfType<IHasSettings>())
          filter.SettingsChanged += Filter_SettingsChanged;
      }
      else if(e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var filter in e.OldItems.OfType<IHasSettings>())
          filter.SettingsChanged -= Filter_SettingsChanged;
      }

      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Filter_SettingsChanged(object sender, EventArgs e)
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}