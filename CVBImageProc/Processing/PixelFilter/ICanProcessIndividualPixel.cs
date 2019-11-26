using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  interface ICanProcessIndividualPixel
  {
    PixelFilterChain PixelFilter { get; set; }
  }
}