using CVBImageProcLib.Processing;
using CVBImageProcLib.Processing.Automation;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLibTest.Processing.Automation
{
  [TestFixture]
  class PropertyAutomationTest
  {
    [Test]
    public void Test()
    {
      var pa = new PropertyAutomation("ValueProvider.FixedValue");
      var math = new Math();
      pa.Parent = math;
    }
  }
}