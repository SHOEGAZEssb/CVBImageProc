using CVBImageProcLib;
using CVBImageProcLib.Processing;
using NUnit.Framework;
using Stemmer.Cvb;

namespace CVBImageProcLibTest.Processing
{
	[TestFixture]
	class MathTest
	{
		[Test]
		public void ProcessingAddWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = true,
				Mode = MathMode.Add
			};
			gain.ValueProvider.FixedValue = 12;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 12, 22, 32, 255 }));
		}

		[Test]
		public void ProcessingAddNoWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = false,
				Mode = MathMode.Add
			};
			gain.ValueProvider.FixedValue = 12;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 12, 22, 32, 6 }));
		}

		[Test]
		public void ProcessingSubtractWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = true,
				Mode = MathMode.Subtract
			};
			gain.ValueProvider.FixedValue = 12;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 0, 8, 238 }));
		}

		[Test]
		public void ProcessingSubtractNoWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = false,
				Mode = MathMode.Subtract
			};
			gain.ValueProvider.FixedValue = 12;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 244, 254, 8, 238 }));
		}

		[Test]
		public void ProcessingMultiplyWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = true,
				Mode = MathMode.Multiply
			};
			gain.ValueProvider.FixedValue = 2;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 20, 40, 255 }));
		}

		[Test]
		public void ProcessingMultiplyNoWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = false,
				Mode = MathMode.Multiply
			};
			gain.ValueProvider.FixedValue = 2;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 20, 40, 244 }));
		}

		[Test]
		public void ProcessingDivideWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = true,
				Mode = MathMode.Divide
			};
			gain.ValueProvider.FixedValue = 2;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 5, 10, 125 }));
		}

		[Test]
		public void ProcessingDivideNoWrapAroundTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = false,
				Mode = MathMode.Divide
			};
			gain.ValueProvider.FixedValue = 2;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 5, 10, 125 }));
		}

		[Test]
		public void ProcessingDivideByZeroTest()
		{
			// given: test image
			byte[] pixelValues = [0, 10, 20, 250];
			using var img = TestHelper.CreateMonoTestImage(pixelValues);
			var gain = new Math
			{
				WrapAround = true,
				Mode = MathMode.Divide
			};
			gain.ValueProvider.FixedValue = 0;

			// when: applying the processor
			using Image processedImage = gain.Process(img);
			// then: gain applied
			Assert.That(processedImage.GetPixels(), Is.EquivalentTo(new byte[] { 0, 10, 20, 250 }));
		}
	}
}