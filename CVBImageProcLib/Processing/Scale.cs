using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor that scales an image.
  /// </summary>
  [DataContract]
  public class Scale : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of this processor.
    /// </summary>
    public string Name => "Scale";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var newImage = new Image(NewSize, inputImage.Planes.Count);

      double scaleX = NewSize.Width / (double)inputImage.Width;
      double scaleY = NewSize.Height / (double)inputImage.Height;
      for (int i = 0; i < inputImage.Planes.Count; i++)
      {
        byte[] inputBytes = inputImage.Planes[i].GetPixels().ToArray();
        if (newImage.Planes[i].TryGetLinearAccess(out LinearAccessData newData))
        {
          var newXInc = (int)newData.XInc;
          var newYInc = (int)newData.YInc;

          unsafe
          {
            var newPBase = (byte*)newData.BasePtr;

            for (int y = 0; y < newImage.Height; y++)
            {
              byte* newPLine = newPBase + newYInc * y;
              var yUnscaled = (int)(y / scaleY);

              for (int x = 0; x < newImage.Width; x++)
              {
                var xUnscaled = (int)(x / scaleX);
                byte* newPPixel = newPLine + newXInc * x;
                *newPPixel = inputBytes[yUnscaled * inputImage.Width + xUnscaled];
              }
            }
          }
        }
        else
          throw new ArgumentException($"New image plane {i} could not be accessed linear", nameof(inputImage));
      }

      return newImage;
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Size to scale input image to.
    /// </summary>
    [DataMember]
    public Size2D NewSize { get; set; } = new Size2D(1, 1);

    #endregion Properties
  }
}