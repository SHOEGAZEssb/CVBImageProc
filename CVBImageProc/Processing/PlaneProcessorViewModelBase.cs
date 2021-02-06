using CVBImageProc.Processing.Automation;
using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Base ViewModel for processors that process
  /// individual planes.
  /// </summary>
  abstract class PlaneProcessorViewModelBase : AutomatableProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// Index of the plane to process.
    /// </summary>
    public int PlaneIndex
    {
      get => _processor.PlaneIndex;
      set
      {
        if (PlaneIndex != value)
        {
          _processor.PlaneIndex = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// If true, all planes are processed.
    /// </summary>
    public bool ProcessAllPlanes
    {
      get => _processor.ProcessAllPlanes;
      set
      {
        if(ProcessAllPlanes != value)
        {
          _processor.ProcessAllPlanes = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor.
    /// </summary>
    private readonly IProcessIndividualPlanes _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The actual processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    protected PlaneProcessorViewModelBase(IProcessIndividualPlanes processor, bool isActive)
      : base(processor, isActive)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}