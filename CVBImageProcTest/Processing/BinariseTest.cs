﻿using CVBImageProc;
using CVBImageProc.Processing;
using NUnit.Framework;

namespace CVBImageProcTest.Processing
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
        var processor = new Binarise()
        {
          Threshold = 100
        };

        // when: applying processor
        using (var processedImage = processor.Process(img))
        {
          // then: binarised
          CollectionAssert.AreEqual(new byte[] { 0, 255, 255, 0 }, processedImage.GetPixels());
        }
      }
    }
  }
}