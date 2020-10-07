using CVBImageProcLib.Processing.PixelFilter;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.PixelFilter
{
  /// <summary>
  /// Tests for the <see cref="LargerThanValue"/> filter.
  /// </summary>
  [TestFixture]
  class LargerThanValueTest
  {
    [Test]
    public void CheckTest()
    {
      // given: LargerThanValue
      var f = new LargerThanValue()
      {
        CompareByte = 10,
        Invert = false
      };

      // when: checking value
      // then: value passes
      Assert.That(f.Check(11), Is.True);
      // then: different value doesn't pass
      Assert.That(f.Check(5), Is.False);
    }

    [Test]
    public void CheckInvertTest()
    {
      // given: LargerThanValue
      var f = new LargerThanValue()
      {
        CompareByte = 10,
        Invert = true
      };

      // when: checking value
      // then: value doesn't pass
      Assert.That(f.Check(11), Is.False);
      // then: different values pass
      Assert.That(f.Check(5), Is.True);
    }
  }
}