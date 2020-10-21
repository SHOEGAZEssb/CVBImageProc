using CVBImageProcLib.Processing.PixelFilter;
using CVBImageProcLib.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Math mode to use while processing.
  /// </summary>
  public enum MathMode
  {
    /// <summary>
    /// Adds a value to the pixel.
    /// </summary>
    Add,

    /// <summary>
    /// Subtracts a value from the pixel.
    /// </summary>
    Subtract,

    /// <summary>
    /// Divides the pixel value.
    /// </summary>
    Divide,

    /// <summary>
    /// Multiplies the pixel value.
    /// </summary>
    Multiply
  }

  /// <summary>
  /// Applies mathematical operations on an image.
  /// </summary>
  [DataContract]
  public class Math : IAOIPlaneProcessor, ICanProcessIndividualPixel
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Math";

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

      Func<int, byte, byte> calculationFunc = MakeCalculationFunc(Mode, WrapAround);

      ProcessingHelper.ProcessMono(inputImage.Planes[PlaneIndex], this.GetProcessingBounds(inputImage), (b) =>
      {
        return calculationFunc(ValueProvider.Provide(), b);
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
    /// Math mode to use while processing.
    /// </summary>
    [DataMember]
    public MathMode Mode { get; set; }

    /// <summary>
    /// If true, pixel values wrap
    /// around at &lt; 0 and &gt; 255.
    /// </summary>
    [DataMember]
    public bool WrapAround { get; set; }

    /// <summary>
    /// The gain value to apply.
    /// </summary>
    [DataMember]
    public IntValueProvider ValueProvider { get; private set; } = new IntValueProvider(0, 255);

    #endregion Properties

    private static Func<int, byte, byte> MakeCalculationFunc(MathMode mode, bool wrapAround)
    {
      Func<int, byte> wrapAroundFunc = MakeWrapAroundFunc(wrapAround);
      Func<int, byte, byte> calculationFunc;
      switch (mode)
      {
        case MathMode.Add:
          calculationFunc = (providedValue, inputByte) => wrapAroundFunc(inputByte + providedValue);
          break;
        case MathMode.Subtract:
          calculationFunc = (providedValue, inputByte) => wrapAroundFunc(inputByte - providedValue);
          break;
        case MathMode.Divide:
          calculationFunc = (providedValue, inputByte) =>
          {
            if (providedValue == 0)
              return inputByte;
            else
              return wrapAroundFunc(inputByte / providedValue);
          };
          break;
        case MathMode.Multiply:
          calculationFunc = (providedValue, inputByte) => wrapAroundFunc(inputByte * providedValue);
          break;
        default:
          throw new ArgumentException("Unknown math mode");
      }

      return calculationFunc;
    }

    private static Func<int, byte> MakeWrapAroundFunc(bool wrapAround)
    {
      if (wrapAround)
      {
        return (calculatedValue) =>
        {
          if (calculatedValue > 255)
            return 255;
          else if (calculatedValue < 0)
            return 0;
          else
            return (byte)calculatedValue;
        };
      }
      else
        return (calculatedValue) => (byte)calculatedValue;
    }
  }
}