using CVBImageProcLib.Processing.PixelFilter;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.PixelFilter
{
	/// <summary>
	/// Tests for the <see cref="LargerThanIndex"/> filter.
	/// </summary>
	[TestFixture]
	class LargerThanIndexTest
	{
		[Test]
		public void CheckTest()
		{
			// given: LargerThanIndex
			var f = new LargerThanIndex()
			{
				CompareValue = 10,
				Invert = false
			};

			// when: checking value
			// then: value passes
			Assert.That(f.Check(11), Is.True);
			// then: different value doesn't pass
			Assert.That(f.Check(5), Is.False);
		}

		[Test]
		public void CheckInvertTest()
		{
			// given: LargerThanIndex
			var f = new LargerThanIndex()
			{
				CompareValue = 10,
				Invert = true
			};

			// when: checking value
			// then: value doesn't pass
			Assert.That(f.Check(11), Is.False);
			// then: different values pass
			Assert.That(f.Check(5), Is.True);
		}
	}
}