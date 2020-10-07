using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;
using Stemmer.Cvb;

namespace CVBImageProcLibTest.Processing
{
  /// <summary>
  /// Tests for the <see cref="Crop"/> processor.
  /// </summary>
  [TestFixture]
  class CropTest
  {
    [Test]
    public void ProcessingMonoTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 1, 2, 3, 4 }))
      {
        var crop = new Crop()
        {
          AOI = new Rect(new Point2D(0, 1), new Size2D(2, 1))
        };

        // when: processing
        using (var processedImage = crop.Process(img))
        {
          // then: cropped image
          CollectionAssert.AreEqual(new byte[] { 3, 4 }, processedImage.GetPixels());
        }
      }
    }
  }
}