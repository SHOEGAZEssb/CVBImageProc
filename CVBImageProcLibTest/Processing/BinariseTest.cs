using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing
{
  /// <summary>
  /// Tests for the <see cref="Binarise"/> processor.
  /// </summary>
  [TestFixture]
  class BinariseTest
  {
    [Test]
    public void ProcessingMonoTest()
    {
      // given: mono test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 10, 200, 100, 0 }))
      {
        var processor = new Binarise();
        processor.Threshold.FixedValue = 100;

        // when: applying processor
        using (var processedImage = processor.Process(img))
        {

          var bla = processedImage.GetPixels();
          // then: binarised
          Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 255, 255, 0 }));
        }
      }
    }
  }
}