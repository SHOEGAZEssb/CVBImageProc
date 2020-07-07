using CVBImageProc.MVVM.Converter;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CVBImageProcTest.Converter
{
  /// <summary>
  /// Tests for the <see cref="ImplementsTypeConverter"/>
  /// </summary>
  [TestFixture]
  class ImplementsTypeConverterTest
  {
    [Test]
    public void ConvertTrueTest()
    {
      // given: converter
      var conv = new ImplementsTypeConverter();

      // when: converting
      bool res = (bool)conv.Convert(new List<object>(), null, typeof(IEnumerable), null);

      // then: true
      Assert.That(res, Is.True);
    }

    [Test]
    public void ConvertFalseTest()
    {
      // given: converter
      var conv = new ImplementsTypeConverter();

      // when: converting
      bool res = (bool)conv.Convert(new List<object>(), null, typeof(IDisposable), null);

      // then: true
      Assert.That(res, Is.False);
    }
  }
}