using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor that shifts pixels.
  /// </summary>
  [DataContract]
  [DisplayName("Pixel Shift")]
  public class PixelShift : FullProcessorBase
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Pixel Shift";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var newImage = new Image(inputImage.Size, inputImage.Planes.Count);
      for (int i = 0; i < newImage.Planes.Count; i++)
      {
        if (UseFillValue && (ProcessAllPlanes || i == PlaneIndex))
          newImage.Planes[i].Initialize(FillValue);
        else
          inputImage.Planes[i].CopyTo(newImage.Planes[i]);
      }

      var bounds = this.GetProcessingBounds(inputImage);
      if (ProcessAllPlanes)
      {
        for (int i = 0; i < inputImage.Planes.Count; i++)
        {
          ProcessPlane(inputImage.Planes[i], newImage.Planes[i], bounds);
        }
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex], newImage.Planes[PlaneIndex], bounds);

      return newImage;
    }

    private void ProcessPlane(ImagePlane inputPlane, ImagePlane newPlane, ProcessingBounds bounds)
    {
      if (inputPlane.TryGetLinearAccess(out LinearAccessData inputData))
      {
        if (newPlane.TryGetLinearAccess(out LinearAccessData newData))
        {
          var inputYInc = (int)inputData.YInc;
          var inputXInc = (int)inputData.XInc;
          var newYInc = (int)newData.YInc;
          var newXInc = (int)newData.XInc;
          int boundsY = bounds.StartY + bounds.Height;
          int boundsX = bounds.StartX + bounds.Width;

          unsafe
          {
            var inputPBase = (byte*)inputData.BasePtr;
            var newPBase = (byte*)newData.BasePtr;

            for (int y = bounds.StartY; y < boundsY; y++)
            {
              byte* inputPLine = inputPBase + inputYInc * y;
              byte* newPLine;
              int newY = y + ShiftY;
              if (newY < boundsY)
                newPLine = newPBase + newYInc * newY;
              else if (Wrap)
                newPLine = newPBase + newYInc * (newY - boundsY);
              else
                continue;

              for (int x = bounds.StartX; x < boundsX; x++)
              {
                byte* inputPPixel = inputPLine + inputXInc * x;
                byte* newPPixel;
                int newX = x + ShiftX;
                if (newX < boundsX)
                  newPPixel = newPLine + newXInc * newX;
                else if (Wrap)
                  newPPixel = newPLine + newXInc * (newX - boundsX);
                else
                  continue;

                if (PixelFilter.Check(*inputPPixel, y * boundsY + x))
                  *newPPixel = *inputPPixel;
              }
            }
          }
        }
        else
          throw new ArgumentException("New plane could not be accessed linear");
      }
      else
        throw new ArgumentException("Input plane could not be accessed linear");
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// The shift in X-direction.
    /// </summary>
    [DataMember]
    public int ShiftX { get; set; }

    /// <summary>
    /// The shift in Y-direction.
    /// </summary>
    [DataMember]
    public int ShiftY { get; set; }

    /// <summary>
    /// Gets if pixels should "wrap" around
    /// back to the beginning of the image.
    /// </summary>
    [DataMember]
    public bool Wrap { get; set; }

    /// <summary>
    /// Gets if the processed plane should be
    /// initialized with the<see cref="FillValue"/>.
    /// If not the original plane is used.
    /// </summary>
    [DataMember]
    public bool UseFillValue { get; set; }

    /// <summary>
    /// Value to fill empty pixels with.
    /// </summary>
    [DataMember]
    public byte FillValue { get; set; }

    #endregion Properties
  }
}