using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.PixelFilter
{
  interface IPixelFilterViewModel : INotifyPropertyChanged, IHasSettings
  {
    string Name { get; }
  }
}