using NUnit.Framework;
using System.Linq;
using Stemmer.Cvb;
using CVBImageProc.Processing;

namespace CVBImageProcTest.Processing
{
  [TestFixture]
  class GainTest
  {
    [Test]
    public void ProcessingTest()
    {
      // given: test image
      byte[] pixelValues = new byte[4] { 0, 10, 20, 30 };
      using(var img = TestHelper.CreateMonoTestImage(pixelValues))
      {
        var gain = new Gain();
        gain.GainValue = 12;

        // when: applying the processor
        using (Image processedImage = gain.Process(img))
        {
          // then: gain applied
          CollectionAssert.AreEqual(pixelValues.Select(b => b + gain.GainValue), processedImage.GetPixels());
        }
      }
    }
  }
}