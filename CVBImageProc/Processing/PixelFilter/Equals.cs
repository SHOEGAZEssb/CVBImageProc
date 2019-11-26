using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  class Equals : IPixelFilter
  {
    #region IPixelFilter Implementation

    public string Name => "Equals";

    public bool Check(byte pixel)
    {
      return pixel == CompareByte;
    }

    #endregion IPixelFilter Implementation

    #region Properties

    public byte CompareByte { get; set; }

    #endregion Properties
  }
}
