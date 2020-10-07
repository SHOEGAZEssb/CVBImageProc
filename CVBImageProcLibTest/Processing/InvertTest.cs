using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing
{
  /// <summary>
  /// Tests for the <see cref="Invert"/> processor.
  /// </summary>
  [TestFixture]
  class InvertTest
  {
    [Test]
    public void ProcessingTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 10, 50, 255, 0 }))
      {
        var invert = new Invert();

        // when: applying the processor
        using (var processedImage = invert.Process(img))
        {
          // then: pixels inverted
          CollectionAssert.AreEqual(new byte[] { 245, 205, 0, 255 }, processedImage.GetPixels());
        }
      }
    }
  }
}