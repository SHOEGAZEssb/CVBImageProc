using Stemmer.Cvb;
using System.Collections.Generic;

namespace CVBImageProc.Processing
{
  class ProcessorChain
  {
    #region Properties

    /// <summary>
    /// List of processors to use.
    /// </summary>
    public List<IProcessor> Processors { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProcessorChain()
    {
      Processors = new List<IProcessor>();
    }

    #endregion Construction

    /// <summary>
    /// Runs the <paramref name="inputImage"/> through
    /// the processor chain.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      Image outputImage = inputImage.Clone();
      foreach (IProcessor processor in Processors)
        outputImage = processor.Process(outputImage);

      return outputImage;
    }
  }
}