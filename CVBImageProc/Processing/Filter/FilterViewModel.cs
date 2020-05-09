using CVBImageProc.MVVM;
using CVBImageProc.Processing.PixelFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// ViewModel for <see cref="IFilter"/> processors.
  /// </summary>
  class FilterViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// List of available filter types.
    /// </summary>
    public static IEnumerable<TypeViewModel> AvailableFilterTypes { get; }

    /// <summary>
    /// The selected filter type.
    /// </summary>
    public TypeViewModel SelectedFilterType
    {
      get => AvailableFilterTypes.FirstOrDefault(f => f.Type == _processor.SelectedFilter?.GetType());
      set
      {
        if(SelectedFilterType != value)
        {
          if (value != null)
            _processor.SelectedFilter = (IFilter)value.Instanciate();
          else
            _processor.SelectedFilter = null;

          OnSettingsChanged();
          NotifyOfPropertyChange();
          NotifyOfPropertyChange(nameof(Name));
          CustomSettingsViewModel = MakeCustomSettingsViewModel();
        }
      }
    }

    /// <summary>
    /// ViewModel for the custom settings of the filter.
    /// </summary>
    public SettingsViewModelBase CustomSettingsViewModel
    {
      get => _customSettingsViewModel;
      set
      {
        if(CustomSettingsViewModel != value)
        {
          if (CustomSettingsViewModel != null)
            CustomSettingsViewModel.SettingsChanged -= SubVM_SettingsChanged;
          if (value != null)
            value.SettingsChanged += SubVM_SettingsChanged;
          _customSettingsViewModel = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private SettingsViewModelBase _customSettingsViewModel;

    /// <summary>
    /// Kernel size to use for the filter.
    /// </summary>
    public KernelSize KernelSize
    {
      get => _processor.KernelSize;
      set
      {
        if(KernelSize != value)
        {
          _processor.KernelSize = value;
          if (CustomSettingsViewModel is INeedKernelSizeUpdate v)
            v.Update();
          
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    /// <summary>
    /// ViewModel for the AOI.
    /// </summary>
    public AOIViewModel AOIVM { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly FilterProcessor _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Static constructor.
    /// </summary>
    static FilterViewModel()
    {
      AvailableFilterTypes = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(mytype => mytype.GetInterfaces().Contains(typeof(IFilter)) && !mytype.IsInterface && !mytype.IsAbstract)
                            .Select(i => new TypeViewModel(i)).ToArray();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public FilterViewModel(FilterProcessor processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
      AOIVM = new AOIViewModel(_processor);
      AOIVM.SettingsChanged += SubVM_SettingsChanged;

      if (processor.SelectedFilter == null)
        SelectedFilterType = AvailableFilterTypes.FirstOrDefault();
      else
        CustomSettingsViewModel = MakeCustomSettingsViewModel();
    }

    #endregion Construction

    /// <summary>
    /// Creates the custom settings ViewModel
    /// for the current configuration.
    /// </summary>
    /// <returns></returns>
    private SettingsViewModelBase MakeCustomSettingsViewModel()
    {
      if (SelectedFilterType == null || SelectedFilterType.Type.GetCustomAttribute<CustomFilterSettingsAttribute>() == null)
        return null;

      switch (_processor.SelectedFilter)
      {
        case Custom c:
          return new CustomKernelSettingsViewModel(c);
        case Median m:
          return new MedianSettingsViewModel(m);
        default:
          return null;
      }
    }

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SubVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}