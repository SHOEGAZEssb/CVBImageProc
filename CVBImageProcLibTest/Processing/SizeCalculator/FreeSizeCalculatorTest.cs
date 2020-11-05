using CVBImageProcLib.Processing.SizeCalculator;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.SizeCalculator
{
  /// <summary>
  /// Tests for the <see cref="FreeSizeCalculator"/>.
  /// </summary>
  [TestFixture]
  class FreeSizeCalculatorTest
  {
    [Test]
    public void GetCalculatedSizeTest()
    {
      // given: image and size calculator
      var calc = new FreeSizeCalculator
      {
        Width = 64,
        Height = 64
      };

      // when: getting size
      var size = calc.GetCalculatedSize(null);

      // then: size is correct
      Assert.That(size.Width, Is.EqualTo(calc.Width));
      Assert.That(size.Height, Is.EqualTo(calc.Height));
    }
  }
}