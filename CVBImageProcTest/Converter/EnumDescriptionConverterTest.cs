using CVBImageProc.MVVM.Converter;
using NUnit.Framework;

namespace CVBImageProcTest.Converter
{
  /// <summary>
  /// Tests for the <see cref="EnumDescriptionConverter"/>.
  /// </summary>
  [TestFixture]
  class EnumDescriptionConverterTest
  {
    private const string TEST1DESCRIPTION = "Test Enum Value";

    private enum TestEnum
    {
      [System.ComponentModel.Description(TEST1DESCRIPTION)]
      Test1,
      Test2
    }

    [Test]
    public void ConvertTest()
    {
      // given: converter
      var conv = new EnumDescriptionConverter();

      // when: converting with attribute
      var res = conv.Convert(TestEnum.Test1, null, null, null);

      // then: correct description
      Assert.That(res, Is.EqualTo(TEST1DESCRIPTION));
    }

    [Test]
    public void ConvertNoAttributeTest()
    {
      // given: converter
      var conv = new EnumDescriptionConverter();

      // when: converting without attribute
      var res = conv.Convert(TestEnum.Test2, null, null, null);

      // then: ToString result
      Assert.That(res, Is.EqualTo(TestEnum.Test2.ToString()));
    }
  }
}