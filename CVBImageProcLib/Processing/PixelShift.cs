using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  [DataContract]
  [DisplayName("Pixel Shift")]
  public class PixelShift : AOIPlaneProcessorBase, ICanProcessIndividualPixel
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
        if (i == PlaneIndex && UseFillValue)
          newImage.Planes[i].Initialize(FillValue);
        else
          inputImage.Planes[i].CopyTo(newImage.Planes[i]);
      }

      if (inputImage.Planes[PlaneIndex].TryGetLinearAccess(out LinearAccessData inputData))
      {
        if (newImage.Planes[PlaneIndex].TryGetLinearAccess(out LinearAccessData newData))
        {
          var inputYInc = (int)inputData.YInc;
          var inputXInc = (int)inputData.XInc;
          var newYInc = (int)newData.YInc;
          var newXInc = (int)newData.XInc;
          ProcessingBounds bounds = this.GetProcessingBounds(inputImage);
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

      return newImage;
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

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