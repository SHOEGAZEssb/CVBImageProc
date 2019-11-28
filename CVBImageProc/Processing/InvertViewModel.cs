using CVBImageProc.Processing.PixelFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing
{
  class InvertViewModel : PlaneProcessorViewModelBase
  {
    #region Properties

    /// <summary>
    /// ViewModel for the processors pixel filter chain.
    /// </summary>
    public PixelFilterChainViewModel PixelFilterChainVM { get; }

    #endregion Properties

    public InvertViewModel(Invert processor) 
      : base(processor)
    {
      PixelFilterChainVM = new PixelFilterChainViewModel(processor.PixelFilter);
      PixelFilterChainVM.SettingsChanged += PixelFilterChainVM_SettingsChanged;
    }

    private void PixelFilterChainVM_SettingsChanged(object sender, EventArgs e)
    {
      OnSettingsChanged();
    }
  }
}
