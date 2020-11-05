using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// The direction to smear.
  /// </summary>
  public enum SmearMode
  {
    /// <summary>
    /// Smear left pixels in
    /// horizontal direction.
    /// </summary>
    HorizontalFromLeft,

    /// <summary>
    /// Smear right pixels in
    /// horizontal direction.
    /// </summary>
    HorizontalFromRight,

    /// <summary>
    /// Smear top pixels in
    /// vertical direction.
    /// </summary>
    VerticalFromTop,

    /// <summary>
    /// Smear bottom pixels in
    /// vertical direction.
    /// </summary>
    VerticalFromBottom
  }

  /// <summary>
  /// Processor that "smears" pixels in a direction.
  /// </summary>
  [DataContract]
  public class Smear : AOIPlaneProcessorBase, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Smear";

    /// <summary>
    /// Applies the gain value
    /// to the given <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to apply gain to.</param>
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
      var valueDic = BuildValueDictionary(Mode, plane, bounds);

      ProcessingHelper.ProcessMono(plane, bounds, (b, y, x) =>
      {
        return (Mode == SmearMode.VerticalFromTop || Mode == SmearMode.VerticalFromBottom) ? valueDic[x] : valueDic[y];
      }, PixelFilter);
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