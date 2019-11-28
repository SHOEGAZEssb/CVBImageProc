using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Logic to use when checking.
  /// </summary>
  enum LogicMode
  {
    /// <summary>
    /// All filter checks need to pass.
    /// </summary>
    And,

    /// <summary>
    /// Only one filter check needs to pass.
    /// </summary>
    Or
  }

  /// <summary>
  /// Filter chain for processors.
  /// </summary>
  [DataContract]
  class PixelFilterChain
  {
    #region Properties

    /// <summary>
    /// Logic used when checking.
    /// </summary>
    [DataMember]
    public LogicMode Mode { get; set; }

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
      if (Filters.Count == 0)
        return true;

      if (Mode == LogicMode.And)
      {
        foreach (var filter in Filters)
        {
          if (!filter.Check(pixel))
            return false;
        }
      }
      else
      {
        foreach(var filter in Filters)
        {
          if (filter.Check(pixel))
            return true;
        }

        return false;
      }

      return true;
    }
  }
}