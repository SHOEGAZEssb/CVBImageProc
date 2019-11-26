using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  class PixelFilterChain
  {
    #region Properties

    public List<IPixelFilter> Filters { get; private set; }

    #endregion Properties

    #region Construction

    public PixelFilterChain()
    {
      Filters = new List<IPixelFilter>();
    }

    #endregion Construction

    /// <summary>
    /// Checks if the given pixel passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel value to check.</param>
    /// <returns>True if <paramref name="pixel"/> passes the filter.</returns>
    public bool Check(byte pixel)
    {
      foreach(var filter in Filters)
      {
        if (!filter.Check(pixel))
          return false;
      }

      return true;
    }
  }
}