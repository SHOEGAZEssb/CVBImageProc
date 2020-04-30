using NUnit.Framework;
using Stemmer.Cvb;
using CVBImageProc.Processing;

namespace CVBImageProcTest.Processing
{
  [TestFixture]
  class GainTest
  {
    [Test]
    public void ProcessingNoWrapAroundTest()
    {
      // given: test image
      byte[] pixelValues = new byte[4] { 0, 10, 20, 250 };
      using(var img = TestHelper.CreateMonoTestImage(pixelValues))
      {
        var gain = new Gain
        {
          WrapAround = false
        };
        gain.ValueProvider.FixedValue = 12;

        // when: applying the processor
        using (Image processedImage = gain.Process(img))
        {
          // then: gain applied
          CollectionAssert.AreEqual(new byte[] { 12, 22, 32, 255 }, processedImage.GetPixels());
        }
      }
    }

    [Test]
    public void ProcessingWrapAroundTest()
    {
      // given: test image
      byte[] pixelValues = new byte[4] { 0, 10, 20, 250 };
      using (var img = TestHelper.CreateMonoTestImage(pixelValues))
      {
        var gain = new Gain
        {
          WrapAround = true
        };
        gain.ValueProvider.FixedValue = 12;

        // when: applying the processor
        using (Image processedImage = gain.Process(img))
        {
          // then: gain applied
          CollectionAssert.AreEqual(new byte[] { 12, 22, 32, 6 }, processedImage.GetPixels());
        }
      }
    }
  }
}