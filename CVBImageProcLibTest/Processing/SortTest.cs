using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing
{
	/// <summary>
	/// Tests for the <see cref="Sort"/> processor.
	/// </summary>
	[TestFixture]
	class SortTest
	{
		[Test]
		public void ProcessAscendingTest()
		{
			// given: test image
			using var img = TestHelper.CreateMonoTestImage([10, 5, 30, 1]);
			var sort = new Sort()
			{
				Mode = SortMode.Ascending
			};

			// when: applying the processor
			using var processedImage = sort.Process(img);
			// then: pixels sorted
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 1, 5, 10, 30 }));
		}

		[Test]
		public void ProcessDescendingTest()
		{
			// given: test image
			using var img = TestHelper.CreateMonoTestImage([10, 5, 30, 1]);
			var sort = new Sort()
			{
				Mode = SortMode.Descending
			};

			// when: applying the processor
			using var processedImage = sort.Process(img);
			// then: pixels sorted
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 30, 10, 5, 1 }));
		}
	}
}