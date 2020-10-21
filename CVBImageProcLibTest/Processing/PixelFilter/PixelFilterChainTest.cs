using CVBImageProcLib.Processing.PixelFilter;
using Moq;
using NUnit.Framework;

namespace CVBImageProcLibTest.Processing.PixelFilter
{
  /// <summary>
  /// Tests for the <see cref="PixelFilterChain"/>
  /// </summary>
  [TestFixture]
  class PixelFilterChainTest
  {
    [Test]
    public void CheckEmptyTest()
    {
      // given: empty chain
      var chain = new PixelFilterChain();

      // when: checking with empty chain
      // then: passes
      Assert.That(chain.Check(0, 0), Is.True);
    }

    [Test]
    public void CheckPassingAndTest()
    {
      // given: chain with filters
      var chain = new PixelFilterChain()
      {
        Mode = LogicMode.And
      };

      var valueFilter1 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter1.Setup(v => v.Check(It.IsAny<byte>())).Returns(true);
      var valueFilter2 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter2.Setup(v => v.Check(It.IsAny<byte>())).Returns(true);
      var indexFilter1 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter1.Setup(i => i.Check(It.IsAny<int>())).Returns(true);
      var indexFilter2 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter2.Setup(i => i.Check(It.IsAny<int>())).Returns(true);

      chain.AddPixelFilter(valueFilter1.Object);
      chain.AddPixelFilter(valueFilter2.Object);
      chain.AddPixelFilter(indexFilter1.Object);
      chain.AddPixelFilter(indexFilter2.Object);

      // when: checking
      // then: passes
      Assert.That(chain.Check(0, 0), Is.True);
    }

    [Test]
    public void CheckFailingAndTest()
    {
      // given: chain with filters
      var chain = new PixelFilterChain()
      {
        Mode = LogicMode.And
      };

      var valueFilter1 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter1.Setup(v => v.Check(It.IsAny<byte>())).Returns(true);
      var valueFilter2 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter2.Setup(v => v.Check(It.IsAny<byte>())).Returns(true);
      var indexFilter1 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter1.Setup(i => i.Check(It.IsAny<int>())).Returns(true);
      var indexFilter2 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter2.Setup(i => i.Check(It.IsAny<int>())).Returns(false);

      chain.AddPixelFilter(valueFilter1.Object);
      chain.AddPixelFilter(valueFilter2.Object);
      chain.AddPixelFilter(indexFilter1.Object);
      chain.AddPixelFilter(indexFilter2.Object);

      // when: checking
      // then: fails
      Assert.That(chain.Check(0, 0), Is.False);
    }

    [Test]
    public void CheckPassingOrTest()
    {
      // given: chain with filters
      var chain = new PixelFilterChain()
      {
        Mode = LogicMode.Or
      };

      var valueFilter1 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter1.Setup(v => v.Check(It.IsAny<byte>())).Returns(false);
      var valueFilter2 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter2.Setup(v => v.Check(It.IsAny<byte>())).Returns(false);
      var indexFilter1 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter1.Setup(i => i.Check(It.IsAny<int>())).Returns(false);
      var indexFilter2 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter2.Setup(i => i.Check(It.IsAny<int>())).Returns(true);

      chain.AddPixelFilter(valueFilter1.Object);
      chain.AddPixelFilter(valueFilter2.Object);
      chain.AddPixelFilter(indexFilter1.Object);
      chain.AddPixelFilter(indexFilter2.Object);

      // when: checking
      // then: passes
      Assert.That(chain.Check(0, 0), Is.True);
    }

    [Test]
    public void CheckFailingOrTest()
    {
      // given: chain with filters
      var chain = new PixelFilterChain()
      {
        Mode = LogicMode.Or
      };

      var valueFilter1 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter1.Setup(v => v.Check(It.IsAny<byte>())).Returns(false);
      var valueFilter2 = new Mock<IPixelValueFilter>(MockBehavior.Strict);
      valueFilter2.Setup(v => v.Check(It.IsAny<byte>())).Returns(false);
      var indexFilter1 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter1.Setup(i => i.Check(It.IsAny<int>())).Returns(false);
      var indexFilter2 = new Mock<IPixelIndexFilter>(MockBehavior.Strict);
      indexFilter2.Setup(i => i.Check(It.IsAny<int>())).Returns(false);

      chain.AddPixelFilter(valueFilter1.Object);
      chain.AddPixelFilter(valueFilter2.Object);
      chain.AddPixelFilter(indexFilter1.Object);
      chain.AddPixelFilter(indexFilter2.Object);

      // when: checking
      // then: fails
      Assert.That(chain.Check(0, 0), Is.False);
    }
  }
}