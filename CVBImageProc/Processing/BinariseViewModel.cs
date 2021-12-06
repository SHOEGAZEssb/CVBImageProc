using CVBImageProc.Processing.ValueProvider;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the <see cref="Binarise"/> processor.
  /// </summary>
  internal class BinariseViewModel : FullProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Threshold of the binarization.
    /// Values &lt; will be set to 0.
    /// Values &gt;= will be set to 255.
    /// </summary>
    public ValueProviderViewModel<int> Threshold { get; }

    /// <summary>
    /// Value to use if the byte value is larger or equal than the <see cref="Threshold"/>.
    /// </summary>
    public ValueProviderViewModel<byte> AboveThresholdValue { get; }

    /// <summary>
    /// Value to use if the byte value is smaller than the <see cref="Threshold"/>.
    /// </summary>
    public ValueProviderViewModel<byte> BelowThresholdValue { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// The actual processor.
    /// </summary>
    private readonly Binarise _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The binarise processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="processor"/> is null.</exception>
    public BinariseViewModel(Binarise processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
      Threshold = new ValueProviderViewModel<int>(_processor.Threshold);
      Threshold.SettingsChanged += SubVM_SettingsChanged;
      AboveThresholdValue = new ValueProviderViewModel<byte>(_processor.AboveThresholdValue);
      AboveThresholdValue.SettingsChanged += SubVM_SettingsChanged;
      BelowThresholdValue = new ValueProviderViewModel<byte>(_processor.BelowThresholdValue);
      BelowThresholdValue.SettingsChanged += SubVM_SettingsChanged;
    }

    #endregion Construction

    /// <summary>
    /// Fires the SettingsChanged event when the
    /// pixel filter settings changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SubVM_SettingsChanged(object sender, System.EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}