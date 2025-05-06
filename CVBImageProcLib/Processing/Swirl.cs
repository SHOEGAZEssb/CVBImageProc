using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using SystemMath = System.Math;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor that swirls an image.
  /// </summary>
  public sealed class Swirl : FullProcessorBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// The name of this processor.
    /// </summary>
    public override string Name => "Swirl";

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
        var cX = imageWidth / 2.0;
        var cY = imageHeight / 2.0;

        unsafe
        {
          var pBase = (byte*)data.BasePtr;
          for (int y = startY; y < boundsY; y++)
          {
            byte* pLine = pBase + yInc * y;
            double relY = cY - y;

            for (int x = startX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              double relX = x - cX;

              double originalAngle;
              if (relX != 0)
              {
                originalAngle = SystemMath.Atan(SystemMath.Abs(relY) / SystemMath.Abs(relX));
                if (relX > 0 && relY < 0)
                  originalAngle = 2.0 * SystemMath.PI - originalAngle;
                else if (relX <= 0 && relY >= 0)
                  originalAngle = SystemMath.PI - originalAngle;
                else if (relX <= 0 && relY < 0)
                  originalAngle += SystemMath.PI;
              }
              else
                originalAngle = relY >= 0 ? 0.5 * SystemMath.PI : 1.5 * SystemMath.PI;

              double radius = SystemMath.Sqrt(relX * relX + relY * relY);
              double newAngle = originalAngle + Factor * radius;
              int srcX = (int)SystemMath.Floor(radius * SystemMath.Cos(newAngle) + 0.5);
              int srcY = (int)SystemMath.Floor(radius * SystemMath.Sin(newAngle) + 0.5);
              srcX += (int)cX;
              srcY += (int)cY;
              srcY = imageHeight - srcY;

              if (srcX < 0)
                srcX = 0;
              else if (srcX >= imageWidth)
                srcX = imageWidth - 1;

              if (srcY < 0)
                srcY = 0;
              else if (srcY >= imageHeight)
                srcY = imageHeight - 1;

              byte* pPixelSwirled = pBase + yInc * srcY + xInc * srcX;
              *pPixel = *pPixelSwirled;
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
    /// Factor by which to swirl.
    /// </summary>
    public double Factor { get; set; }

    #endregion Properties
  }
}
