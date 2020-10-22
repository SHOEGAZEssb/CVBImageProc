using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.SizeCalculator
{
  /// <summary>
  /// Size calculator based on percentage.
  /// </summary>
  [DataContract]
  [DisplayName("Percentage")]
  public class PercentageSizeCalculator : ISizeCalculator
  {
    #region Properties

    /// <summary>
    /// The percentage to use of the input size.
    /// </summary>
    public double Percentage
    {
      get => _percentage;
      set
      {
        if (value < 0.0)
          value = 0.0;

        _percentage = value;
      }
    }
    [DataMember]
    private double _percentage = 1.0;

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
      if (img == null)
        throw new ArgumentNullException(nameof(img));

      return new Size2D((int)(img.Width * Percentage), (int)(img.Height * Percentage));
    }

    #endregion ISizeCalculator Implementation
  }
}