using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLib.Processing.Automation.ValueProvider
{
  public enum NumericValueProvideMode
  {
    Up,
    Down,
    UpDown,
    Random
  }

  public interface IAutomationValueProvider
  {
    object ProvideNextValue();
  }
}