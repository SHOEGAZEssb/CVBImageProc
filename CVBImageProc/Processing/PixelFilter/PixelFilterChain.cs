using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Filter chain for processors.
  /// </summary>
  [DataContract]
  class PixelFilterChain
  {
    #region Properties

    /// <summary>
    /// The configured filters.
    /// </summary>
    [DataMember]
    public List<IPixelFilter> Filters { get; private set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PixelFilterChain()
    {
      Filters = new List<IPixelFilter>();
    }

    #endregion Construction

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel to check.</param>
    /// <returns>True if the <paramref name="pixel"/> passes
    /// the filter, otherwise false.</returns>
    public bool Check(byte pixel)
    {
      foreach (var filter in Filters)
      {
        if (!filter.Check(pixel))
          return false;
      }

      return true;
    }
  }
}