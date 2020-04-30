using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.ValueProvider
{
  interface IValueProvider<T> where T : struct
  {
    #region Properties

    T FixedValue { get; set; }

    T MinValue { get; }

    T MaxValue { get; }

    bool Randomize { get; set; }

    T MinRandomValue { get; set; }

    T MaxRandomValue { get; set; }

    #endregion Properties

    T Provide();
  }
}
