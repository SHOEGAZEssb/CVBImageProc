using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Algorithm to use for scaling.
  /// </summary>
  public enum ScaleMode
  {
    /// <summary>
    /// Use nearest-neighbor interpolation.
    /// </summary>
    NearestNeighbor,

    Bilinear
  }

  /// <summary>
  /// Processor that scales an image.
  /// </summary>
  [DataContract]
  public class Resize : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of this processor.
    /// </summary>
    public string Name => "Resize";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (Mode == ScaleMode.NearestNeighbor)
        return ProcessNearestNeighbor(inputImage);
      else if (Mode == ScaleMode.Bilinear)
        return ProcessBiliniar(inputImage);
      else
        throw new ArgumentException("Unknown scale mode");
    }

    /// <summary>
    /// Scales the image using nearest neighbor interpolation.
    /// </summary>
    /// <param name="inputImage">Image to scale.</param>
    /// <returns>Scaled image.</returns>
    private Image ProcessNearestNeighbor(Image inputImage)
    {
      var newImage = new Image(NewSize, inputImage.Planes.Count);

      double scaleX = NewSize.Width / (double)inputImage.Width;
      double scaleY = NewSize.Height / (double)inputImage.Height;
      for (int i = 0; i < inputImage.Planes.Count; i++)
      {
        byte[,] inputBytes = inputImage.Planes[i].GetPixelsAs2DArray();
        ProcessingHelper.ProcessMono(newImage.Planes[i], (b, y, x) =>
        {
          var yUnscaled = (int)(y / scaleY);
          var xUnscaled = (int)(x / scaleX);
          return inputBytes[yUnscaled, xUnscaled];
        });
      }

      return newImage;
    }

    /// <summary>
    /// Scales the image using bilinear interpolation.
    /// </summary>
    /// <param name="inputImage">Image to scale.</param>
    /// <returns>Scaled image.</returns>
    private Image ProcessBiliniar(Image inputImage)
    {
      var newImage = new Image(NewSize, inputImage.Planes.Count);

      double scaleX = (double)(inputImage.Width - 1) / NewSize.Width;
      double scaleY = (double)(inputImage.Height - 1) / NewSize.Height;
      for (int i = 0; i < inputImage.Planes.Count; i++)
      {
        byte[,] inputBytes = inputImage.Planes[i].GetPixelsAs2DArray();
        ProcessingHelper.ProcessMono(newImage.Planes[i], (b, y, x) =>
        {
          var pY = (int)(scaleY * y);
          double yDiff = (scaleY * y) - pY;
          var pX = (int)(scaleX * x);
          double xDiff = (scaleX * x) - pX;

          return (byte)(inputBytes[pY, pX] * (1 - xDiff) * (1 - yDiff) +
                        inputBytes[pY, pX + 1] * (1 - yDiff) * xDiff +
                        inputBytes[pY + 1, pX] * yDiff * (1 - xDiff) +
                        inputBytes[pY + 1, pX + 1] * yDiff * xDiff);
        });
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

    /// <summary>
    /// The algorithm to use for scaling.
    /// </summary>
    [DataMember]
    public ScaleMode Mode { get; set; } = ScaleMode.NearestNeighbor;

    #endregion Properties
  }
}