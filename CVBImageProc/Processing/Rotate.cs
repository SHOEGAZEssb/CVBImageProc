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
        Image rotatedImage = new Image(inputImage.Size, inputImage.Planes.Count);
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
        int height = inputImage.Height;
        int width = inputImage.Width;
        double midPointY = height / 2;
        double midPointX = width / 2;
        double cos = Math.Cos(Angle.Rad);
        double sin = Math.Sin(Angle.Rad);
        unsafe
        {
          for (int y = 0; y < height; y++)
          {
            byte* inputPLine = (byte*)(inputData.BasePtr + inputYInc * y);
             
            for (int x = 0; x < width; x++)
            {
              byte* inputPPixel = inputPLine + inputXInc * x;

              int newX = (int)((x - midPointX) * cos - (y - midPointY) * sin + midPointX);
              int newY = (int)((x - midPointX) * sin + (y - midPointY) * cos + midPointY);

              if (newX < width && newX >= 0 && newY < height && newY >= 0 && PixelFilter.Check(*inputPPixel, y * height + x))
              {
                byte* rotatedPPixel = (byte*)(rotatedData.BasePtr + rotatedYInc * newY + rotatedXInc * newX);
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
    /// Fill value for "empty" pixels.
    /// </summary>
    [DataMember]
    public byte FillValue { get; set; }

    #endregion Properties
  }
}