using CVBImageProc.Processing;
using NUnit.Framework;

namespace CVBImageProcTest.Processing
{
  /// <summary>
  /// Tests for the <see cref="Replace"/> processor.
  /// </summary>
  [TestFixture]
  class ReplaceTest
  {
    [Test]
    public void ProcessingTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 10, 50, 255, 0 }))
      {
        var replace = new Replace();
        replace.ValueProvider.FixedValue = 42;

        // when: applying processor
        using (var processedImage = replace.Process(img))
        {
          // then: pixels replaced
          CollectionAssert.AreEqual(new byte[] { 42, 42, 42, 42 }, processedImage.GetPixels());
        }
      }
    }
  }
}