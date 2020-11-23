using Stemmer.Cvb;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
  /// <summary>
  /// Base class for filter processors.
  /// </summary>
  [DataContract]
  [SubProcessor]
  public abstract class FilterBase : FullProcessorBase, IFilter
  {
    #region IFilter

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public override abstract string Name { get; }

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public override abstract Image Process(Image inputImage);

    #endregion IFilter

    #region Properties

    /// <summary>
    /// Kernel size to use.
    /// </summary>
    [DataMember]
    public virtual KernelSize KernelSize { get; set; }

    #endregion Properties
  }
}