using CVBImageProc.Processing.PixelFilter;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
  abstract class FullProcessorViewModelBase : AOIPlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    #endregion Properties

    #region Construction

    protected FullProcessorViewModelBase(IFullProcessor processor, bool isActive)
      : base(processor, isActive)
    {
      PixelFilterChainVM = new PixelFilterChainViewModel(processor);
      PixelFilterChainVM.SettingsChanged += PixelFilterChainVM_SettingsChanged;
    }

    #endregion Construction

    private void PixelFilterChainVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}