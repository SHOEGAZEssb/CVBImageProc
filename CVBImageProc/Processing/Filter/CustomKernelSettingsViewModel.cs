using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CVBImageProc.Processing.Filter
{
  /// <summary>
  /// ViewModel for the settings of a <see cref="Custom"/> filter processor.
  /// </summary>
  class CustomKernelSettingsViewModel : SettingsViewModelBase, INeedKernelSizeUpdate
  {
    #region Properties

    /// <summary>
    /// The custom weights of the processor.
    /// </summary>
    public ObservableCollection<KernelValueViewModel> Weights { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly Custom _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public CustomKernelSettingsViewModel(Custom processor)
    {
      _processor = processor ?? throw new ArgumentNullException(nameof(processor));
      Weights = new ObservableCollection<KernelValueViewModel>(_processor.Weights.Select(w => new KernelValueViewModel(w)));
      foreach(var w in Weights)
        w.SettingsChanged += KernelValue_SettingsChanged;
    }

    #endregion Construction

    /// <summary>
    /// Tells this object to update its
    /// settings according to the kernel size
    /// of its filter.
    /// </summary>
    public void Update()
    {
      foreach(var w in Weights.ToArray())
      {
        w.SettingsChanged -= KernelValue_SettingsChanged;
        Weights.Remove(w);
      }

      foreach(var v in _processor.Weights)
      {
        var vm = new KernelValueViewModel(v);
        vm.SettingsChanged += KernelValue_SettingsChanged;
        Weights.Add(vm);
      }
    }

    private void KernelValue_SettingsChanged(object sender, EventArgs e)
    {
      if (!(sender is KernelValueViewModel vm))
        return;

      _processor.Weights[Weights.IndexOf(vm)] = vm.Value;
      OnSettingsChanged();
    }
  }
}