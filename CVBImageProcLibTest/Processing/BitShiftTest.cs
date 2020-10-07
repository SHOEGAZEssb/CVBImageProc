using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing
{
  [TestFixture]
  class BitShiftTest
  {
    [Test]
    public void ProcessingLeftNoWrapAroundTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 0, 1, 10, 255 }))
      {
        var p = new BitShift()
        {
          ShiftDirection = BitShiftDirection.Left,
          WrapAround = false
        };
        p.ValueProvider.FixedValue = 1;

        // when: applying the processor
        using (var processedImage = p.Process(img))
        {
          // then: pixels bit shifted
          CollectionAssert.AreEqual(new byte[] { 0, 2, 20, 254 }, processedImage.GetPixels());
        }
      }
    }

    [Test]
    public void ProcessingLeftWrapAroundTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 0, 1, 10, 255 }))
      {
        var p = new BitShift()
        {
          ShiftDirection = BitShiftDirection.Left,
          WrapAround = true
        };
        p.ValueProvider.FixedValue = 1;

        // when: applying the processor
        using (var processedImage = p.Process(img))
        {
          // then: pixels bit shifted
          CollectionAssert.AreEqual(new byte[] { 0, 2, 20, 255 }, processedImage.GetPixels());
        }
      }
    }

    [Test]
    public void ProcessingRightTest()
    {
      // given: test image
      using (var img = TestHelper.CreateMonoTestImage(new byte[] { 0, 1, 10, 255 }))
      {
        var p = new BitShift()
        {
          ShiftDirection = BitShiftDirection.Right
        };
        p.ValueProvider.FixedValue = 1;

        // when: applying the processor
        using (var processedImage = p.Process(img))
        {
          // then: pixels bit shifted
          CollectionAssert.AreEqual(new byte[] { 0, 0, 5, 127 }, processedImage.GetPixels());
        }
      }
    }
  }
}