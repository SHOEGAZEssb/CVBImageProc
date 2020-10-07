using CVBImageProcLib.Processing.ValueProvider;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.ValueProvider
{
  /// <summary>
  /// Tests for the <see cref="ByteValueProvider"/>.
  /// </summary>
  [TestFixture]
  class ByteValueProviderTest
  {
    [Test]
    public void ProvideFixedValueTest()
    {
      // given: value provider
      var p = new ByteValueProvider(0, 255)
      {
        FixedValue = 42
      };

      // when: providing value
      // then: correct value provided
      Assert.That(p.Provide(), Is.EqualTo(42));
    }
  }
}