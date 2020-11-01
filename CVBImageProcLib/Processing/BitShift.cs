using CVBImageProcLib.Processing.PixelFilter;
using CVBImageProcLib.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Direction of a bit shift operation.
  /// </summary>
  public enum BitShiftDirection
  {
    /// <summary>
    /// Bits should be shifted left.
    /// </summary>
    Left,

    /// <summary>
    /// Bits should be shifted right.
    /// </summary>
    Right
  }

  /// <summary>
  /// Processor that shifts bits.
  /// </summary>
  [DataContract]
  [DisplayName("Bit Shift")]
  public class BitShift : AOIPlaneProcessorBase, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override string Name => "Bit Shift";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (ProcessAllPlanes)
      {
        foreach (var plane in inputImage.Planes)
          ProcessPlane(plane);
      }
      else
        ProcessPlane(inputImage.Planes[PlaneIndex]);

      return inputImage;
    }

    private void ProcessPlane(ImagePlane plane)
    {
      ProcessingHelper.ProcessMono(plane, this.GetProcessingBounds(plane.Parent), (b) =>
      {
        if (ShiftDirection == BitShiftDirection.Left)
        {
          int pixelValue = b << ValueProvider.Provide();
          if (WrapAround && pixelValue > 255)
            return 255;
          else
            return (byte)pixelValue;
        }
        else
          return (byte)(b >> ValueProvider.Provide());
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
    /// Direction to shift bits.
    /// </summary>
    [DataMember]
    public BitShiftDirection ShiftDirection { get; set; }

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    [DataMember]
    public bool WrapAround { get; set; }

    /// <summary>
    /// The amount to shift.
    /// </summary>
    [DataMember]
    public IntValueProvider ValueProvider { get; private set; } = new IntValueProvider(0, 255);

    #endregion Properties
  }
}
