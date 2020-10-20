using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  public enum SmearMode
  {
    HorizontalFromLeft,
    HorizontalFromRight,
    VerticalFromTop,
    VerticalFromBottom
  }

  [DataContract]
  public class Smear : IAOIPlaneProcessor, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Smear";

    /// <summary>
    /// Applies the gain value
    /// to the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to apply gain to.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      var valueDic = BuildValueDictionary(Mode, inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage));

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b, y, x) =>
      {
        return (Mode == SmearMode.VerticalFromTop || Mode == SmearMode.VerticalFromBottom) ? valueDic[x] : valueDic[y];
      }, PixelFilter);

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; set; } = new PixelFilterChain();

    #endregion ICanProcessIndividualPixel Implementation

    #region ICanProcessIndividualRegions Implementation

    /// <summary>
    /// If true, uses the <see cref="AOI"/>
    /// while processing.
    /// </summary>
    [DataMember]
    public bool UseAOI { get; set; }

    /// <summary>
    /// The AOI to process.
    /// </summary>
    [DataMember]
    public Rect AOI { get; set; }

    #endregion ICanProcessIndividualRegions Implementation

    #region IProcessIndividualPlanes Implementation

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    [DataMember]
    public int PlaneIndex { get; set; }

    #endregion IProcessIndividualPlanes Implementation

    #region Properties

    /// <summary>
    /// Direction to smear pixels in.
    /// </summary>
    [DataMember]
    public SmearMode Mode { get; set; } = SmearMode.VerticalFromTop;

    #endregion Properties

    private static IDictionary<int, byte> BuildValueDictionary(SmearMode mode, ImagePlane plane, ProcessingBounds bounds)
    {
      var dic = new Dictionary<int, byte>();
      if (mode == SmearMode.VerticalFromTop)
      {
        for (int x = bounds.StartX; x < bounds.StartX + bounds.Width; x++)
          dic.Add(x, (byte)plane.GetPixel(x, bounds.StartY));
      }
      else if (mode == SmearMode.VerticalFromBottom)
      {
        for (int x = bounds.StartX; x < bounds.StartX + bounds.Width; x++)
          dic.Add(x, (byte)plane.GetPixel(x, bounds.StartY + bounds.Height - 1));
      }
      else if (mode == SmearMode.HorizontalFromLeft)
      {
        for (int y = bounds.StartY; y < bounds.StartY + bounds.Height; y++)
          dic.Add(y, (byte)plane.GetPixel(bounds.StartX, y));
      }
      else if (mode == SmearMode.HorizontalFromRight)
      {
        for (int y = bounds.StartY; y < bounds.StartY + bounds.Height; y++)
          dic.Add(y, (byte)plane.GetPixel(bounds.StartX + bounds.Width - 1, y));
      }
      else
        throw new ArgumentException("Unknown Smear Mode", nameof(mode));

      return dic;
    }
  }
}