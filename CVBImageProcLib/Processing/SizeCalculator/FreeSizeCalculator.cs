using Stemmer.Cvb;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.SizeCalculator
{
  /// <summary>
  /// Size calculator using free width and height input.
  /// </summary>
  [DataContract]
  [DisplayName("Free")]
  public class FreeSizeCalculator : ISizeCalculator
  {
    #region Properties

    /// <summary>
    /// The new width of the size.
    /// </summary>
    public int Width
    {
      get => _width;
      set
      {
        if (value < 1)
          value = 1;

        _width = value;
      }
    }
    [DataMember]
    private int _width = 1;

    /// <summary>
    /// The new height of the size.
    /// </summary>
    public int Height
    {
      get => _height;
      set
      {
        if (value < 1)
          value = 1;

        _height = value;
      }
    }
    [DataMember]
    private int _height = 1;

    #endregion Properties

    #region ISizeCalculator Implementation

    /// <summary>
    /// Gets the calculated size for
    /// the given <paramref name="img"/>.
    /// </summary>
    /// <param name="img">Img to calculate size for.</param>
    /// <returns>Calculated size.</returns>
    public Size2D GetCalculatedSize(Image img)
    {
      return new Size2D(Width, Height);
    }

    #endregion ISizeCalculator Implementation
  }
}