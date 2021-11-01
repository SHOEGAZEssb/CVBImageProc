using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  class SwirlViewModel : FullProcessorViewModelBase
  {
    #region Properties

    public double Factor
    {
      get => _processor.Factor;
      set
      {
        if(Factor != value)
        {
          _processor.Factor = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    private readonly Swirl _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="isActive">Default active state.</param>
    public SwirlViewModel(Swirl processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}
