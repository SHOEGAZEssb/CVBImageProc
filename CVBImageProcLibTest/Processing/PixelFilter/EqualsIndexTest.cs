using CVBImageProcLib.Processing.PixelFilter;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.PixelFilter
{
	[TestFixture]
	class EqualsIndexTest
	{
		[Test]
		public void CheckTest()
		{
			// given: EqualsIndex
			var f = new EqualsIndex()
			{
				CompareValue = 42,
				Invert = false
			};

			// when: checking value
			// then: value passes
			Assert.That(f.Check(42), Is.True);
			// then: different value doesn't pass
			Assert.That(f.Check(43), Is.False);
		}

		[Test]
		public void CheckInvertedTest()
		{
			// given: EqualsIndex
			var f = new EqualsIndex()
			{
				CompareValue = 42,
				Invert = true
			};

			// when: checking value
			// then: value doesn't pass
			Assert.That(f.Check(42), Is.False);
			// then: different value passes
			Assert.That(f.Check(43), Is.True);
		}
	}
}