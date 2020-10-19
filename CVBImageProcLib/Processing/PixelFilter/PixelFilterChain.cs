using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.PixelFilter
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
    /// All configured filters.
    /// </summary>
    public IEnumerable<KeyValuePair<IPixelFilter, bool>> Filters => ValueFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))
                                                            .Concat(IndexFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))
                                                            .Concat(AutoFilters.Select(kvp => new KeyValuePair<IPixelFilter, bool>(kvp.Key, kvp.Value))));

    /// <summary>
    /// The configured value filters.
    /// </summary>
    [DataMember]
    public List<KeyValuePair<IPixelValueFilter, bool>> ValueFilters { get; private set; }

    /// <summary>
    /// The configured index filters.
    /// </summary>
    [DataMember]
    public List<KeyValuePair<IPixelIndexFilter, bool>> IndexFilters { get; private set; }

    /// <summary>
    /// The configured auto filters.
    /// </summary>
    [DataMember]
    public List<KeyValuePair<IPixelAutoFilter, bool>> AutoFilters { get; private set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PixelFilterChain()
    {
      ValueFilters = new List<KeyValuePair<IPixelValueFilter, bool>>();
      IndexFilters = new List<KeyValuePair<IPixelIndexFilter, bool>>();
      AutoFilters = new List<KeyValuePair<IPixelAutoFilter, bool>>();
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
      var valueFilters = ValueFilters.Where(v => v.Value).Select(v => v.Key);
      var indexFilters = IndexFilters.Where(v => v.Value).Select(v => v.Key);
      var autoFilters = AutoFilters.Where(v => v.Value).Select(v => v.Key);

      if (valueFilters.Count() == 0 && indexFilters.Count() == 0 && autoFilters.Count() == 0)
        return true;

      if (Mode == LogicMode.And)
      {
        foreach (var filter in valueFilters)
        {
          if (!filter.Check(pixel))
            return false;
        }
        foreach (var filter in indexFilters)
        {
          if (!filter.Check(index))
            return false;
        }
        foreach (var filter in autoFilters)
        {
          if (!filter.Check())
            return false;
        }
      }
      else
      {
        foreach (var filter in valueFilters)
        {
          if (filter.Check(pixel))
            return true;
        }
        foreach (var filter in indexFilters)
        {
          if (filter.Check(index))
            return true;
        }
        foreach (var filter in autoFilters)
        {
          if (filter.Check())
            return true;
        }

        return false;
      }

      return true;
    }

    /// <summary>
    /// Checks if the given rgb pixel pass the filter.
    /// </summary>
    /// <param name="r">R pixel to check.</param>
    /// <param name="g">G pixel to check.</param>
    /// <param name="b">B pixel to check.</param>
    /// <param name="index">Pixel index to check.</param>
    /// <returns>True if the given pixels meet the filter conditions.</returns>
    public bool Check(byte r, byte g, byte b, int index)
    {
      var valueFilters = ValueFilters.Where(v => v.Value).Select(v => v.Key);
      var indexFilters = IndexFilters.Where(v => v.Value).Select(v => v.Key);
      var autoFilters = AutoFilters.Where(v => v.Value).Select(v => v.Key);

      if (valueFilters.Count() == 0 && indexFilters.Count() == 0 && autoFilters.Count() == 0)
        return true;

      if (Mode == LogicMode.And)
      {
        foreach (var filter in valueFilters)
        {
          if (!filter.Check(r) || !filter.Check(g) || !filter.Check(b))
            return false;
        }
        foreach (var filter in indexFilters)
        {
          if (!filter.Check(index))
            return false;
        }
        foreach (var filter in autoFilters)
        {
          if (!filter.Check())
            return false;
        }
      }
      else
      {
        foreach (var filter in valueFilters)
        {
          if (filter.Check(r) && filter.Check(g) && filter.Check(b))
            return true;
        }
        foreach (var filter in indexFilters)
        {
          if (filter.Check(index))
            return true;
        }
        foreach (var filter in autoFilters)
        {
          if (filter.Check())
            return true;
        }

        return false;
      }

      return true;
    }
  }
}