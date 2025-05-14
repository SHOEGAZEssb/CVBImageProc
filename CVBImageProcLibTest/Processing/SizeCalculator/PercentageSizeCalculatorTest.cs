using CVBImageProcLib.Processing.SizeCalculator;
using NUnit.Framework;
using Stemmer.Cvb;

namespace CVBImageProcLibTest.Processing.SizeCalculator
{
	/// <summary>
	/// Tests for the <see cref="PercentageSizeCalculator"/>.
	/// </summary>
	[TestFixture]
	class PercentageSizeCalculatorTest
	{
		[Test]
		public void GetCalculatedSizeTest()
		{
			// given: image and calculator
			using var img = new Image(128, 128);
			var calc = new PercentageSizeCalculator
			{
				Percentage = 0.5
			};

			// when: getting size
			var size = calc.GetCalculatedSize(img);

			// then: size correct
			Assert.That(size.Width, Is.EqualTo(img.Width * calc.Percentage));
			Assert.That(size.Height, Is.EqualTo(img.Height * calc.Percentage));
		}

		[Test]
		public void GetCalculatedSizeNoImageTest()
		{
			// given: calculator
			var calc = new PercentageSizeCalculator();

			// when: getting size with no image
			// then: ArgumentNullException
			Assert.That(() => calc.GetCalculatedSize(null), Throws.ArgumentNullException);
		}
	}
}