using CVBImageProc.Processing.ValueProvider;
using NUnit.Framework;

namespace CVBImageProcTest.Processing.ValueProvider
{
  /// <summary>
  /// Tests for the <see cref="IntValueProvider"/>.
  /// </summary>
  [TestFixture]
  class IntValueProviderTest
  {
    [Test]
    public void ProvideFixedValueTest()
    {
      // given: value provider
      var p = new IntValueProvider(0, 255)
      {
        FixedValue = 42
      };

      // when: providing value
      // then: correct value provided
      Assert.That(p.Provide(), Is.EqualTo(42));
    }
  }
}