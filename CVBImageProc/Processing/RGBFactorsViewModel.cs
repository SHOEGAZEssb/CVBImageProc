using CVBImageProc.MVVM;
using CVBImageProc.Processing.PixelFilter;
using CVBImageProcLib.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="RGBFactors"/> processor.
  /// </summary>
  class RGBFactorsViewModel : ProcessorViewModel, IHasSettings
  {
    #region Properties

    /// <summary>
    /// Event that is fired when one of
    /// the settings changed.
    /// </summary>
    public event EventHandler SettingsChanged;

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    /// <summary>
    /// ViewModel for the AOI.
    /// </summary>
    public AOIViewModel AOIVM { get; }

    /// <summary>
    /// List of available presets.
    /// </summary>
    public static IEnumerable<RGBFactorPresetViewModel> AvailablePresets { get; }

    /// <summary>
    /// Currently selected preset.
    /// </summary>
    public RGBFactorPresetViewModel SelectedPreset
    {
      get => _selectedPreset;
      set
      {
        if (SelectedPreset != value)
        {
          _selectedPreset = value;
          LoadPreset();
          SettingsChanged?.Invoke(this, EventArgs.Empty);
        }
      }
    }
    private RGBFactorPresetViewModel _selectedPreset;

    /// <summary>
    /// Factor of the R component of the R pixel.
    /// </summary>
    public double FactorRR
    {
      get => _processor.FactorRR;
      set
      {
        if (FactorRR != value)
        {
          _processor.FactorRR = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the G component of the R pixel.
    /// </summary>
    public double FactorRG
    {
      get => _processor.FactorRG;
      set
      {
        if (FactorRG != value)
        {
          _processor.FactorRG = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the B component of the R pixel.
    /// </summary>
    public double FactorRB
    {
      get => _processor.FactorRB;
      set
      {
        if (FactorRB != value)
        {
          _processor.FactorRB = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the R component of the G pixel.
    /// </summary>
    public double FactorGR
    {
      get => _processor.FactorGR;
      set
      {
        if (FactorGR != value)
        {
          _processor.FactorGR = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the G component of the G pixel.
    /// </summary>
    public double FactorGG
    {
      get => _processor.FactorGG;
      set
      {
        if (FactorGG != value)
        {
          _processor.FactorGG = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the B component of the G pixel.
    /// </summary>
    public double FactorGB
    {
      get => _processor.FactorGB;
      set
      {
        if (FactorGB != value)
        {
          _processor.FactorGB = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the R component of the B pixel.
    /// </summary>
    public double FactorBR
    {
      get => _processor.FactorBR;
      set
      {
        if (FactorBR != value)
        {
          _processor.FactorBR = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the G component of the B pixel.
    /// </summary>
    public double FactorBG
    {
      get => _processor.FactorBG;
      set
      {
        if (FactorBG != value)
        {
          _processor.FactorBG = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the B component of the B pixel.
    /// </summary>
    public double FactorBB
    {
      get => _processor.FactorBB;
      set
      {
        if (FactorBB != value)
        {
          _processor.FactorBB = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    public bool WrapAroundR
    {
      get => _processor.WrapAroundR;
      set
      {
        if (WrapAroundR != value)
        {
          _processor.WrapAroundR = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    public bool WrapAroundG
    {
      get => _processor.WrapAroundG;
      set
      {
        if (WrapAroundG != value)
        {
          _processor.WrapAroundG = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// If true, clamps the pixel values to 255 and 0,
    /// to stop them from overflowing.
    /// </summary>
    public bool WrapAroundB
    {
      get => _processor.WrapAroundB;
      set
      {
        if (WrapAroundB != value)
        {
          _processor.WrapAroundB = value;
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Command for reloading the <see cref="SelectedPreset"/>.
    /// </summary>
    public ICommand ReloadPresetCommand { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly RGBFactors _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Static constructor.
    /// </summary>
    static RGBFactorsViewModel()
    {
      AvailablePresets = new[]
      {
        new RGBFactorPresetViewModel("Mono", new RGBFactorPreset(
          0.2125, 0.7154, 0.0721,
          0.2125, 0.7154, 0.0721,
          0.2125, 0.7154, 0.0721)),
        new RGBFactorPresetViewModel("Sepia", new RGBFactorPreset(
          0.393, 0.769, 0.189,
          0.349, 0.686, 0.168,
          0.272, 0.534, 0.131))
      };
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public RGBFactorsViewModel(RGBFactors processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      PixelFilterChainVM = new PixelFilterChainViewModel(_processor);
      PixelFilterChainVM.SettingsChanged += SubVM_SettingsChanged;
      AOIVM = new AOIViewModel(_processor);
      AOIVM.SettingsChanged += SubVM_SettingsChanged;
      ReloadPresetCommand = new DelegateCommand((o) => ReloadPreset());
      LoadInitialPreset();
    }

    #endregion Construction

    /// <summary>
    /// Sets and loads the initial preset if needed.
    /// </summary>
    private void LoadInitialPreset()
    {
      if (_processor.FactorRR == 0 && _processor.FactorRG == 0 && _processor.FactorRB == 0 &&
         _processor.FactorGR == 0 && _processor.FactorGG == 0 && _processor.FactorGB == 0 &&
         _processor.FactorBR == 0 && _processor.FactorBG == 0 && _processor.FactorBB == 0)
        SelectedPreset = AvailablePresets.First();
      else
        _selectedPreset = AvailablePresets.First();
    }

    /// <summary>
    /// Loads the <see cref="SelectedPreset"/>.
    /// </summary>
    private void LoadPreset()
    {
      _processor.FactorRR = SelectedPreset.Preset.FactorRR;
      _processor.FactorRG = SelectedPreset.Preset.FactorRG;
      _processor.FactorRB = SelectedPreset.Preset.FactorRB;
      _processor.FactorGR = SelectedPreset.Preset.FactorGR;
      _processor.FactorGG = SelectedPreset.Preset.FactorGG;
      _processor.FactorGB = SelectedPreset.Preset.FactorGB;
      _processor.FactorBR = SelectedPreset.Preset.FactorBR;
      _processor.FactorBG = SelectedPreset.Preset.FactorBG;
      _processor.FactorBB = SelectedPreset.Preset.FactorBB;
      NotifyOfPropertyChange(nameof(FactorRR));
      NotifyOfPropertyChange(nameof(FactorRG));
      NotifyOfPropertyChange(nameof(FactorRB));
      NotifyOfPropertyChange(nameof(FactorGR));
      NotifyOfPropertyChange(nameof(FactorGG));
      NotifyOfPropertyChange(nameof(FactorGB));
      NotifyOfPropertyChange(nameof(FactorBR));
      NotifyOfPropertyChange(nameof(FactorBG));
      NotifyOfPropertyChange(nameof(FactorBB));
    }

    /// <summary>
    /// Reloads the <see cref="SelectedPreset"/>.
    /// </summary>
    private void ReloadPreset()
    {
      LoadPreset();
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SubVM_SettingsChanged(object sender, EventArgs e)
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}