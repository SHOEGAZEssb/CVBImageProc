using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// ViewModel for a <see cref="ICanProcessIndividualRegions"/> processor.
  /// </summary>
  class AOIViewModel : SettingsViewModelBase
  {
    #region Properties

    /// <summary>
    /// Indicates if an AOI should be used
    /// </summary>
    public bool UseAOI
    {
      get => _processor.UseAOI;
      set
      {
        if (UseAOI != value)
        {
          _processor.UseAOI = value;
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// X-Coordinate of the top left AOI corner.
    /// </summary>
    public int AOIX
    {
      get => _processor.AOI.Location.X;
      set
      {
        if (AOIX != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(new Point2D(value, aoi.Location.Y), aoi.Size);
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// Y-Coordinate of the top left AOI corner.
    /// </summary>
    public int AOIY
    {
      get => _processor.AOI.Location.Y;
      set
      {
        if (AOIY != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(new Point2D(aoi.Location.X, value), aoi.Size);
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// Width of the AOI.
    /// </summary>
    public int AOIWidth
    {
      get => _processor.AOI.Size.Width;
      set
      {
        if (AOIWidth != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(aoi.Location, new Size2D(value, aoi.Size.Height));
          NotifyOfPropertyChange();
          OnSettingsChanged();
        }
      }
    }

    /// <summary>
    /// Height of the AOI.
    /// </summary>
    public int AOIHeight
    {
      get => _processor.AOI.Size.Height;
      set
      {
        if (AOIHeight != value)
        {
          Rect aoi = _processor.AOI;
          _processor.AOI = new Rect(aoi.Location, new Size2D(aoi.Size.Width, value));
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
    private readonly ICanProcessIndividualRegions _processor;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor.</param>
    public AOIViewModel(ICanProcessIndividualRegions processor)
    {
      _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    #endregion Construction
  }
}