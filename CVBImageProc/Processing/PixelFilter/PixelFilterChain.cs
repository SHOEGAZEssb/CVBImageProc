using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Logic to use when checking.
  /// </summary>
  public enum LogicMode
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
  public class PixelFilterChain
  {
    #region Properties

    /// <summary>
    /// Logic used when checking.
    /// </summary>
    [DataMember]
    public LogicMode Mode { get; set; }

    /// <summary>
    /// The configured value filters.
    /// </summary>
    [DataMember]
    public List<IPixelValueFilter> ValueFilters { get; private set; }

    /// <summary>
    /// The configured index filters.
    /// </summary>
    [DataMember]
    public List<IPixelIndexFilter> IndexFilters { get; private set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PixelFilterChain()
    {
      ValueFilters = new List<IPixelValueFilter>();
      IndexFilters = new List<IPixelIndexFilter>();
    }

    #endregion Construction

    /// <summary>
    /// Checks if the given <paramref name="pixel"/>
    /// passes the filter.
    /// </summary>
    /// <param name="pixel">Pixel value to check.</param>
    /// <param name="index">Pixel index to check.</param>
    /// <returns>True if the <paramref name="pixel"/> and <paramref name="index"/> pass
    /// the filter, otherwise false.</returns>
    public bool Check(byte pixel, int index)
    {
      if (ValueFilters.Count == 0 && IndexFilters.Count == 0)
        return true;

      if (Mode == LogicMode.And)
      {
        foreach (var filter in ValueFilters)
        {
          if (!filter.Check(pixel))
            return false;
        }
        foreach (var filter in IndexFilters)
        {
          if (!filter.Check(index))
            return false;
        }
      }
      else
      {
        foreach (var filter in ValueFilters)
        {
          if (filter.Check(pixel))
            return true;
        }
        foreach (var filter in IndexFilters)
        {
          if (filter.Check(index))
            return true;
        }

        return false;
      }

      return true;
    }
  }
}