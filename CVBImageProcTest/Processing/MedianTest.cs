using CVBImageProc.Processing.Filter;
using NUnit.Framework;
using Stemmer.Cvb;

namespace CVBImageProcTest.Processing
{
  [TestFixture]
  class MedianTest
  {
    [Test]
    public void ProcessingTest()
    {
      using(var img = Image.FromFile(@"E:\Program Files\STEMMER IMAGING\Common Vision Blox\Tutorial\Test5.bmp"))
      {
        var median = new Median();

        using (var processedImage = median.Process(img))
        {
          processedImage.Save("Test.bmp");
        }
      }
    }
  }
}
