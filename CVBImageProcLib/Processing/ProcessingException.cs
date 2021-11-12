using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Exception caused by a processor processing.
  /// </summary>
  [Serializable]
  public class ProcessingException : Exception
  {
    #region Properties

    /// <summary>
    /// The processor that caused the error.
    /// </summary>
    public IProcessor Processor { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="processor">The processor that caused the error.</param>
    /// <param name="message">Exception message.</param>
    public ProcessingException(IProcessor processor, string message)
      : base(message)
    {
      Processor = processor;
    }

    #region Standard Exception Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProcessingException()
    { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public ProcessingException(string message)
      : base(message)
    { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public ProcessingException(string message, Exception innerException)
      : base(message, innerException)
    { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="serializationInfo">SerializationInfo.</param>
    /// <param name="streamingContext">StreamingContext.</param>
    protected ProcessingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
    { }

    #endregion Standard Exception Constructors

    #endregion Construction
  }
}