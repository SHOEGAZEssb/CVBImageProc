using CVBImageProc.MVVM;
using System;
using System.Windows.Input;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="RGBToMono"/> processor.
  /// </summary>
  class RGBToMonoViewModel : ProcessorViewModel, IHasSettings
  {
    #region Properties

    public event EventHandler SettingsChanged;

    /// <summary>
    /// Factor of the R pixel.
    /// </summary>
    public double FactorR
    {
      get => _processor.FactorR;
      set
      {
        if (FactorR != value)
        {
          _processor.FactorR = Math.Round(value, 4);
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the G pixel.
    /// </summary>
    public double FactorG
    {
      get => _processor.FactorG;
      set
      {
        if (FactorG != value)
        {
          _processor.FactorG = Math.Round(value, 4);
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Factor of the B pixel.
    /// </summary>
    public double FactorB
    {
      get => _processor.FactorB;
      set
      {
        if (FactorB != value)
        {
          _processor.FactorB = Math.Round(value, 4);
          SettingsChanged?.Invoke(this, EventArgs.Empty);
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// Command for resetting the <see cref="FactorR"/>
    /// to default value.
    /// </summary>
    public ICommand ResetFactorRCommand { get; }

    /// <summary>
    /// Command for resetting the <see cref="FactorG"/>
    /// to default value.
    /// </summary>
    public ICommand ResetFactorGCommand { get; }

    /// <summary>
    /// Command for resetting the <see cref="FactorB"/>
    /// to default value.
    /// </summary>
    public ICommand ResetFactorBCommand { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly RGBToMono _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    public RGBToMonoViewModel(RGBToMono processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      ResetFactorRCommand = new DelegateCommand((o) => ResetFactorR());
      ResetFactorGCommand = new DelegateCommand((o) => ResetFactorG());
      ResetFactorBCommand = new DelegateCommand((o) => ResetFactorB());
    }

    #endregion Construction

    /// <summary>
    /// Resets the <see cref="FactorR"/>
    /// back to default value;
    /// </summary>
    private void ResetFactorR()
    {
      FactorR = RGBToMono.DEFAULTFACTORRED;
    }

    /// <summary>
    /// Resets the <see cref="FactorG"/>
    /// back to default value;
    /// </summary>
    private void ResetFactorG()
    {
      FactorG = RGBToMono.DEFAULTFACTORGREEN;
    }

    /// <summary>
    /// Resets the <see cref="FactorB"/>
    /// back to default value;
    /// </summary>
    private void ResetFactorB()
    {
      FactorB = RGBToMono.DEFAULTFACTORBLUE;
    }
  }
}
