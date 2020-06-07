using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor that rotates an image.
  /// </summary>
  [DataContract]
  public class Rotate : IProcessor, IProcessIndividualPlanes, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Rotate";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (inputImage.Planes[PlaneIndex].TryGetLinearAccess(out LinearAccessData inputData))
      {
        double midPointY = inputImage.Height / 2;
        double midPointX = inputImage.Width / 2;
        int height = inputImage.Height;
        int width = inputImage.Width;
        double cos = Math.Cos(Angle.Rad);
        double sin = Math.Sin(Angle.Rad);

        Image rotatedImage;
        if (FitImage)
        {
          var newHeight = Math.Abs(width * sin) + Math.Abs(height * cos);
          var newWidth = Math.Abs(width * cos) + Math.Abs(height * sin);

          rotatedImage = new Image((int)newWidth, (int)newHeight, inputImage.Planes.Count);
        }
        else
          rotatedImage = new Image(inputImage.Size, inputImage.Planes.Count);

        for (int i = 0; i < inputImage.Planes.Count; i++)
        {
          if (i != PlaneIndex)
            inputImage.Planes[i].CopyTo(rotatedImage.Planes[i]);
          else
            rotatedImage.Planes[i].Initialize(FillValue);
        }

        var rotatedData = rotatedImage.Planes[PlaneIndex].GetLinearAccess();

        int inputYInc = (int)inputData.YInc;
        int inputXInc = (int)inputData.XInc;
        int rotatedYInc = (int)rotatedData.YInc;
        int rotatedXInc = (int)rotatedData.XInc;
        unsafe
        {
          for (int y = 0; y < height; y++)
          {
            byte* inputPLine = (byte*)(inputData.BasePtr + inputYInc * y);

            for (int x = 0; x < width; x++)
            {
              byte* inputPPixel = inputPLine + inputXInc * x;

              var newP = NormalizePoint(RotatePoint(x, y, midPointX, midPointY, cos, sin));
              if (FitImage)
              {
                if (PixelFilter.Check(*inputPPixel, y * height + x))
                {
                  byte* rotatedPPixel = (byte*)(rotatedData.BasePtr + rotatedYInc * newP.Y + rotatedXInc * newP.X);
                  *rotatedPPixel = *inputPPixel;
                }
              }
              else if (newP.X < width && newP.X >= 0 && newP.Y < height && newP.Y >= 0 && PixelFilter.Check(*inputPPixel, y * height + x))
              {
                byte* rotatedPPixel = (byte*)(rotatedData.BasePtr + rotatedYInc * newP.Y + rotatedXInc * newP.X);
                *rotatedPPixel = *inputPPixel;
              }
            }
          }
        }

        return rotatedImage;
      }
      else
        throw new ArgumentException("Input image could not be accessed linearly", nameof(inputImage));
    }

    private static Point2D RotatePoint(int x, int y, double originX, double originY, double cos, double sin)
    {
      int newX = (int)((x - originX) * cos - (y - originY) * sin + originX);
      int newY = (int)((x - originX) * sin + (y - originY) * cos + originY);
      return new Point2D(newX, newY);
    }

    private static Point2D NormalizePoint(Point2D p)
    {
      if(p.X < 0)
      {
        return new Point2D(p.X - p.X, p.Y);
      }
      if(p.Y < 0)
      {
        return new Point2D(p.X, p.Y - p.Y);
      }

      return p;
    }

    #endregion IProcessor Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    [DataMember]
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region Properties

    /// <summary>
    /// Angle by which to rotate.
    /// </summary>
    [DataMember]
    public Angle Angle { get; set; }

    /// <summary>
    /// If true, fits the rotated image
    /// in the new image.
    /// If false, new image size will be
    /// equal to the size of the input image.
    /// </summary>
    [DataMember]
    public bool FitImage { get; set; }

    /// <summary>
    /// Fill value for "empty" pixels.
    /// </summary>
    [DataMember]
    public byte FillValue { get; set; }

    #endregion Properties
  }
}