using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor that pixelates an image.
  /// </summary>
  [DataContract]
  public class Pixelate : FullProcessorBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// The name of this processor.
    /// </summary>
    public override string Name => "Pixelate";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var bounds = this.GetProcessingBounds(inputImage);
      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane, bounds);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex], bounds);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane, ProcessingBounds bounds)
    {
      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        int startY = bounds.StartY;
        int startX = bounds.StartX;
        int boundsY = startY + bounds.Height;
        int boundsX = startX + bounds.Width;
        int imageHeight = plane.Parent.Height;
        int imageWidth = plane.Parent.Width;
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;

        unsafe
        {
          var pBase = (byte*)data.BasePtr;
          for (int yy = bounds.StartY; yy < boundsY; yy += PixelateSize)
          {
            int offsetY = PixelateSize / 2;
            int yyPixelateSize = yy + PixelateSize;
            for (int xx = bounds.StartX; xx < boundsX; xx += PixelateSize)
            {
              int offsetX = PixelateSize / 2;
              int xxPixelateSize = xx + PixelateSize;

              while (xx + offsetX >= imageWidth)
                offsetX--;
              while (yy + offsetY >= imageHeight)
                offsetY--;

              byte* pPixelatedPixel = pBase + ((yy + offsetY) * yInc) + ((xx + offsetX) * xInc);
              for (int y = yy; y < yyPixelateSize && y < imageHeight; y++)
              {
                byte* pLine = pBase + yInc * y;

                for (int x = xx; x < xxPixelateSize && x < imageWidth; x++)
                {
                  byte* pPixel = pLine + xInc * x;
                  if (PixelFilter.Check(*pPixel, y * boundsY + x))
                    *pPixel = *pPixelatedPixel;
                }
              }
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linearly", nameof(plane));
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Size of each new "pixel".
    /// </summary>
    [DataMember]
    public int PixelateSize { get; set; } = 5;

    #endregion Properties
  }
}