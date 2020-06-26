using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Chain of processors.
  /// </summary>
  [DataContract]
  class ProcessorChain
  {
    #region Properties

    /// <summary>
    /// List of processors to use.
    /// </summary>
    [DataMember]
    public List<KeyValuePair<IProcessor, bool>> Processors { get; private set; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProcessorChain()
    {
      Processors = new List<KeyValuePair<IProcessor, bool>>();
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
      foreach (IProcessor processor in Processors.Where(kvp => kvp.Value).Select(kvp => kvp.Key))
      {
        try
        {
          outputImage = processor.Process(outputImage);
        }
        catch(Exception ex)
        {
          throw new ProcessingException(processor, ex.Message);
        }
      }

      return outputImage;
    }
  }
}