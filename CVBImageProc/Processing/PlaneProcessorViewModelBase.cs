using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Base ViewModel for processors that process
  /// individual planes.
  /// </summary>
  class PlaneProcessorViewModelBase : ProcessorViewModel, INeedImageInfo, IHasSettings
  {
    #region INeedImageInfo Implementation

    public virtual void UpdateImageInfo(Image img)
    {
      MaxPlaneIndex = img.Planes.Count - 1;
      if (PlaneIndex > MaxPlaneIndex)
        PlaneIndex = MaxPlaneIndex;
    }

    #endregion INeedImageInfo Implementation

    #region IHasSettings Implementation

    public event EventHandler SettingsChanged;

    protected void OnSettingsChanged()
    {
      SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion IHasSettings Implementation

    #region Properties

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

    public int MaxPlaneIndex
    {
      get => _maxPlaneIndex;
      private set
      {
        if (MaxPlaneIndex != value)
        {
          _maxPlaneIndex = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private int _maxPlaneIndex;

    #endregion Properties

    #region Member

    private readonly IProcessIndividualPlanes _processor;

    #endregion Member

    #region Construction

    protected PlaneProcessorViewModelBase(IProcessIndividualPlanes processor)
      : base(processor)
    {
      _processor = processor;
    }

    #endregion Construction
  }
}