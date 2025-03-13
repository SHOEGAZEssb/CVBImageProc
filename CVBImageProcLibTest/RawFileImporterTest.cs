using CVBImageProcLib;
using NUnit.Framework;
using Stemmer.Cvb;
using System.Linq;

namespace CVBImageProcLibTest
{
  /// <summary>
  /// Tests for the <see cref="RawFileImporter"/>
  /// </summary>
  [TestFixture]
  class RawFileImporterTest
  {
    [Test]
    public void ImportMonoWithoutFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 10, 128, 255 };
      var size = new Size2D(2, 2);

      // when: importing
      using (var img = RawFileImporter.ImportAsMono(data, size, 42))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(1));
        Assert.That(img.Size, Is.EqualTo(size));
        Assert.That(img.GetPixels(), Is.EquivalentTo(data));
      }
    }

    [Test]
    public void ImportMonoWithoutDataTest()
    {
      // when: trying to import without data
      // then: ArgumentNullException
      Assert.That(() => RawFileImporter.ImportAsMono(null, new Size2D(2, 2), 42), Throws.ArgumentNullException);
    }

    [Test]
    public void ImportMonoWithFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 10, 128 };
      var size = new Size2D(2, 2);
      byte fill = 42;

      // when: importing
      using (var img = RawFileImporter.ImportAsMono(data, size, fill))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(1));
        Assert.That(img.Size, Is.EqualTo(size));
        Assert.That(img.GetPixels(), Is.EquivalentTo(data.Concat(new byte[] { fill })));
      }
    }

    [Test]
    public void ImportRGBRGBRGBWithoutFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 1, 2, 3,
                              4, 5, 6, 7,
                              8, 9, 10, 11 };

      var size = new Size2D(2, 2);
      byte fill = 42;

      // when: importing
      using(var img = RawFileImporter.ImportAsRGB(data, size, fill, RGBMode.RGBRGB))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(3));
        Assert.That(img.Size, Is.EqualTo(size));

        var rData = new byte[] { data[0], data[3], data[6], data[9] };
        var gData = new byte[] { data[1], data[4], data[7], data[10] };
        var bData = new byte[] { data[2], data[5], data[8], data[11] };
        Assert.That(img.Planes[0].GetPixels(), Is.EqualTo(rData));
        Assert.That(img.Planes[1].GetPixels(), Is.EqualTo(gData));
        Assert.That(img.Planes[2].GetPixels(), Is.EqualTo(bData));
      }
    }

    [Test]
    public void ImportRGBRGBRGBWithFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 1, 2, 3,
                              4, 5, 6, 7,
                              8, };

      var size = new Size2D(2, 2);
      byte fill = 42;

      // when: importing
      using (var img = RawFileImporter.ImportAsRGB(data, size, fill, RGBMode.RGBRGB))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(3));
        Assert.That(img.Size, Is.EqualTo(size));

        var rData = new byte[] { data[0], data[3], data[6], fill };
        var gData = new byte[] { data[1], data[4], data[7], fill };
        var bData = new byte[] { data[2], data[5], data[8], fill };
        Assert.That(img.Planes[0].GetPixels(), Is.EqualTo(rData));
        Assert.That(img.Planes[1].GetPixels(), Is.EqualTo(gData));
        Assert.That(img.Planes[2].GetPixels(), Is.EqualTo(bData));
      }
    }

    [Test]
    public void ImportRGBRRGGBBWithoutFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 1, 2, 3,
                              4, 5, 6, 7,
                              8, 9, 10, 11 };

      var size = new Size2D(2, 2);
      byte fill = 42;

      // when: importing
      using (var img = RawFileImporter.ImportAsRGB(data, size, fill, RGBMode.RRGGBB))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(3));
        Assert.That(img.Size, Is.EqualTo(size));
        Assert.That(img.GetPixels(), Is.EquivalentTo(data));
      }
    }

    [Test]
    public void ImportRGBRRGGBBWithFillTest()
    {
      // given: import data
      var data = new byte[] { 0, 1, 2, 3,
                              4, 5, 6, 7,
                              8, };

      var size = new Size2D(2, 2);
      byte fill = 42;

      // when: importing
      using (var img = RawFileImporter.ImportAsRGB(data, size, fill, RGBMode.RRGGBB))
      {
        // then: image correctly imported
        Assert.That(img.Planes.Count, Is.EqualTo(3));
        Assert.That(img.Size, Is.EqualTo(size));
        Assert.That(img.GetPixels(), Is.EquivalentTo(data.Concat(new byte[] { fill, fill, fill })));
      }
    }

    [Test]
    public void ImportRGBWithoutDataTest()
    {
      // when: trying to import without data
      // then: ArgumentNullException
      Assert.That(() => RawFileImporter.ImportAsRGB(null, new Size2D(2, 2), 42, RGBMode.RGBRGB), Throws.ArgumentNullException);
    }
  }
}