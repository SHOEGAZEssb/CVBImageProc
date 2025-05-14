using CVBImageProcLib.Processing.PixelFilter;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.PixelFilter
{
	/// <summary>
	/// Tests for the <see cref="ModuloIndex"/> filter.
	/// </summary>
	[TestFixture]
	class ModuloIndexTest
	{
		[Test]
		public void CheckTest()
		{
			// given: ModuloIndex
			var f = new ModuloIndex()
			{
				CompareValue = 2,
				Invert = false
			};

			// when: checking values
			// then: values pass
			Assert.That(f.Check(0), Is.True);
			Assert.That(f.Check(2), Is.True);
			Assert.That(f.Check(4), Is.True);
			// then: different values don't pass
			Assert.That(f.Check(1), Is.False);
			Assert.That(f.Check(3), Is.False);
			Assert.That(f.Check(5), Is.False);
		}

		[Test]
		public void CheckInvertTest()
		{
			// given: ModuloIndex
			var f = new ModuloIndex()
			{
				CompareValue = 2,
				Invert = true
			};

			// when: checking values
			// then: values don't pass
			Assert.That(f.Check(0), Is.False);
			Assert.That(f.Check(2), Is.False);
			Assert.That(f.Check(4), Is.False);
			// then: different values pass
			Assert.That(f.Check(1), Is.True);
			Assert.That(f.Check(3), Is.True);
			Assert.That(f.Check(5), Is.True);
		}
	}
}