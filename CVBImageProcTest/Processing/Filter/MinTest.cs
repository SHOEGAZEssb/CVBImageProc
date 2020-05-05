using CVBImageProc;
using CVBImageProc.Processing.Filter;
using NUnit.Framework;

namespace CVBImageProcTest.Processing.Filter
{
  /// <summary>
  /// Tests for the <see cref="Min"/> filter.
  /// </summary>
  [TestFixture]
  class MinTest
  {
    [Test]
    public void Processing3x3Test()
    {
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 10, 5, 3, 20, 10, 8, 1, 10, 0 }))
      {
        var min = new Min()
        {
          KernelSize = KernelSize.ThreeByThree
        };

        // when: processing:
        using (var processedImage = min.Process(img))
        {
          // then: min values
          CollectionAssert.AreEqual(new byte[] { 5, 3, 3, 1, 0, 0, 1, 0, 0 }, processedImage.GetPixels());
        }
      }
    }
  }
}