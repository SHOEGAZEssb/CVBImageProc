using CVBImageProc.MVVM;
using CVBImageProcLib.Processing.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// ViewModel for <see cref="IFilter"/> processors.
  /// </summary>
  internal sealed class FilterViewModel : FullProcessorViewModelBase
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
        if (SelectedFilterType != value)
        {
          _processor.SelectedFilter = value == null ? null : (IFilter)value.Instanciate();

          OnSettingsChanged();
          NotifyOfPropertyChange();
          NotifyOfPropertyChange(nameof(Name));
          NotifyOfPropertyChange(nameof(KernelSize));
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
        if (CustomSettingsViewModel != value)
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
        if (KernelSize != value)
        {
          _processor.KernelSize = value;
          if (CustomSettingsViewModel is INeedKernelSizeUpdate v)
            v.Update();

          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

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
      AvailableFilterTypes = Assembly.GetAssembly(typeof(IFilter)).GetTypes()
                            .Where(mytype => mytype.GetInterfaces().Contains(typeof(IFilter)) && !mytype.IsInterface && !mytype.IsAbstract)
                            .Select(i => new TypeViewModel(i)).OrderBy(t => t.Name).ToArray();
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
    /// <returns>ViewModel for the custom settings of the filter.</returns>
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
        case OilPainting o:
          return new OilPaintingSettingsViewModel(o);
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