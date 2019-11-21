using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Processor for binarising an image.
  /// </summary>
  class Binarise : IProcessor
  {
    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Binarise";

    /// <summary>
    /// Binarises the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (inputImage.Planes.Count == 1)
        return BinariseMono(inputImage);
      else
        return inputImage;
    }

    private Image BinariseMono(Image inputImage)
    {
      return inputImage;
    }
  }
}