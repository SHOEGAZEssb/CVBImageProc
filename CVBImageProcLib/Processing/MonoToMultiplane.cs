using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor for converting a mono image
  /// into a multiplane image.
  /// </summary>
  [DataContract]
  [DisplayName("Mono To Multiplane")]
  public class MonoToMultiplane : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Mono To Multiplane";

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));
      if (inputImage.Planes.Count > 1)
        throw new ArgumentException($"Input image is not compatible with {Name}. (Too many planes)", nameof(inputImage));

      return Image.FromPlanes(MappingOption.CopyPixels, Enumerable.Repeat(inputImage.Planes[0], NumPlanes).ToArray());
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Amount of planes in the new image.
    /// </summary>
    [DataMember]
    public int NumPlanes
    {
      get => _numPlanes;
      set
      {
        if (NumPlanes != value)
        {
          if (value < 1)
            value = 1;

          _numPlanes = value;
        }
      }
    }
    private int _numPlanes = 3;

    #endregion Properties
  }
}
