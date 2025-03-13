using CVBImageProcLib;
using CVBImageProcLib.Processing.Filter;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.Filter
{
  /// <summary>
  /// Tests for the <see cref="Max"/> filter.
  /// </summary>
  [TestFixture]
  class MaxTest
  {
    [Test]
    public void Processing3x3Test()
    {
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 10, 5, 3, 20, 10, 8, 1, 10, 30 }))
      {
        var max = new Max()
        {
          KernelSize = KernelSize.ThreeByThree
        };

        // when: processing:
        using (var processedImage = max.Process(img))
        {
          // then: min values
          Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 20, 20, 10, 20, 30, 30, 20, 30, 30 }));
        }
      }
    }
  }
}