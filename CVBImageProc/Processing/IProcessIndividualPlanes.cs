using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing
{
  interface IProcessIndividualPlanes : IProcessor
  {
    int PlaneIndex { get; set; }
  }
}