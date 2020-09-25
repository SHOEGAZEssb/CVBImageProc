using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Base ViewModel for processors that process individual planes
  /// and support AOIs.
  /// </summary>
  abstract class AOIPlaneProcessorViewModelBase : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// ViewModel for the AOI.
    /// </summary>
    public AOIViewModel AOIVM { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The actual processor.</param>
    /// <param name="isActive">Startup IsActive state.</param>
    protected AOIPlaneProcessorViewModelBase(IAOIPlaneProcessor processor, bool isActive)
      : base(processor, isActive)
    {
      AOIVM = new AOIViewModel(processor);
      AOIVM.SettingsChanged += SubVM_SettingsChanged;
    }

    #endregion Construction

    public override void UpdateImageInfo(Image img)
    {
      base.UpdateImageInfo(img);
      if (AOIVM.AOIX + AOIVM.AOIWidth >= img.Width)
        AOIVM.AOIWidth = img.Width - AOIVM.AOIX - 1;
      if (AOIVM.AOIY + AOIVM.AOIHeight >= img.Height)
        AOIVM.AOIHeight = img.Height - AOIVM.AOIY - 1;
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