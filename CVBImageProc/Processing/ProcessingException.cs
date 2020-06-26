using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing
{
  class ProcessingException : Exception
  {
    /// <summary>
    /// The processor that caused the error.
    /// </summary>
    public IProcessor Processor { get; }

    public ProcessingException(IProcessor processor, string message)
      : base(message)
    {
      Processor = processor;
    }
  }
}
