using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing
{
	/// <summary>
	/// Tests for the <see cref="Replace"/> processor.
	/// </summary>
	[TestFixture]
	class ReplaceTest
	{
		[Test]
		public void ProcessingTest()
		{
			// given: test image
			using var img = TestHelper.CreateMonoTestImage([10, 50, 255, 0]);
			var replace = new Replace();
			replace.ValueProvider.FixedValue = 42;

			// when: applying processor
			using var processedImage = replace.Process(img);
			// then: pixels replaced
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 42, 42, 42, 42 }));
		}
	}
}