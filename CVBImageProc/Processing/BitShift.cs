using CVBImageProc.Processing.PixelFilter;
using CVBImageProc.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
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
  class BitShift : IProcessor, ICanProcessIndividualPixel, IProcessIndividualPlanes, ICanProcessIndividualRegions
  {
    #region IProcessor Implementation

    public string Name => "Bit Shift";

    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b) =>
      {
        if (ShiftDirection == BitShiftDirection.Left)
          return (byte)(b << ValueProvider.Provide());
        else
          return (byte)(b >> ValueProvider.Provide());
      });

      return inputImage;
    }

    #endregion IProcessor Implementation

    #region ICanProcessIndividualPixel Implementation

    /// <summary>
    /// Filter chain for the processor.
    /// </summary>
    [DataMember]
    public PixelFilterChain PixelFilter { get; private set; } = new PixelFilterChain();

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
    /// Direction to shift bits.
    /// </summary>
    [DataMember]
    public BitShiftDirection ShiftDirection { get; set; }

    /// <summary>
    /// The amount to shift.
    /// </summary>
    [DataMember]
    public IntValueProvider ValueProvider { get; private set; } = new IntValueProvider(0, 255);

    #endregion Properties
  }
}
