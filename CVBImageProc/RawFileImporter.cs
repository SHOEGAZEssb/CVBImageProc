using CVBImageProc.Processing;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVBImageProc
{
  /// <summary>
  /// How rgb pixels should be read
  /// from the raw bytes.
  /// </summary>
  enum RGBMode
  {
    /// <summary>
    /// Pixels are read in r g b
    /// order after each other.
    /// </summary>
    RGBRGB,

    /// <summary>
    /// R pixels are read fully first,
    /// followed by g and then b in full.
    /// </summary>
    RRGGBB
  }

  /// <summary>
  /// Imports any file as an image.
  /// </summary>
  static class RawFileImporter
  {
    /// <summary>
    /// Imports the given <paramref name="file"/> as a mono image.
    /// </summary>
    /// <param name="file">File to import.</param>
    /// <param name="imageSize">Size of the resulting image.</param>
    /// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
    /// <returns>Imported mono image.</returns>
    public static Image ImportAsMono(string file, Size2D imageSize, byte fill)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException(nameof(file));

      return ImportAsMono(File.ReadAllBytes(file), imageSize, fill);
    }

    /// <summary>
    /// Imports the given <paramref name="rawBytes"/> as mono image.
    /// </summary>
    /// <param name="rawBytes">The raw bytes to use for the image.</param>
    /// <param name="imageSize">Size of the resulting image.</param>
    /// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
    /// <returns>Imported mono image.</returns>
    public static Image ImportAsMono(byte[] rawBytes, Size2D imageSize, byte fill)
    {
      if (rawBytes == null)
        throw new ArgumentNullException(nameof(rawBytes));

      var queue = new Queue<byte>(rawBytes);
      var img = new Image(imageSize);

      ProcessingHelper.ProcessMono(img.Planes[0], (b) =>
      {
        return queue.Any() ? queue.Dequeue() : fill;
      });

      return img;
    }

    /// <summary>
    /// Imports the given <paramref name="file"/> as a mono image.
    /// </summary>
    /// <param name="file">File to import.</param>
    /// <param name="imageSize">Size of the resulting image.</param>
    /// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
    /// <param name="rgbMode">Read-in mode of the rgb bytes.</param>
    /// <returns>Imported rgb image.</returns>
    public static Image ImportAsRGB(string file, Size2D imageSize, byte fill, RGBMode rgbMode)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException(nameof(file));

      return ImportAsRGB(File.ReadAllBytes(file), imageSize, fill, rgbMode);
    }

    /// <summary>
    /// Imports the given <paramref name="rawBytes"/> as rgb image.
    /// </summary>
    /// <param name="rawBytes">The raw bytes to use for the image.</param>
    /// <param name="imageSize">Size of the resulting image.</param>
    /// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
    /// <param name="rgbMode">Read-in mode of the rgb bytes.</param>
    /// <returns>Imported rgb image.</returns>
    public static Image ImportAsRGB(byte[] rawBytes, Size2D imageSize, byte fill, RGBMode rgbMode)
    {
      if (rawBytes == null)
        throw new ArgumentNullException(nameof(rawBytes));

      var queue = new Queue<byte>(rawBytes);
      var img = new Image(imageSize, 3);

      if (rgbMode == RGBMode.RGBRGB)
        ImportRGBAsRGBRGB(queue, img, fill);
      else
        ImportRGBAsRRGGBB(queue, img, fill);

      return img;
    }

    private static void ImportRGBAsRGBRGB(Queue<byte> rawBytes, Image img, byte fill)
    {
      if (rawBytes == null)
        throw new ArgumentNullException(nameof(rawBytes));
      if (img == null)
        throw new ArgumentNullException(nameof(img));

      ProcessingHelper.ProcessRGB(img, (i) =>
      {
        byte r = rawBytes.Any() ? rawBytes.Dequeue() : fill;
        byte g = rawBytes.Any() ? rawBytes.Dequeue() : fill;
        byte b = rawBytes.Any() ? rawBytes.Dequeue() : fill;
        return new Tuple<byte, byte, byte>(r, g, b);
      });
    }

    private static void ImportRGBAsRRGGBB(Queue<byte> rawBytes, Image img, byte fill)
    {
      if (rawBytes == null)
        throw new ArgumentNullException(nameof(rawBytes));
      if (img == null)
        throw new ArgumentNullException(nameof(img));

      ProcessingHelper.ProcessMono(img.Planes[0], (b) =>
      {
        return rawBytes.Any() ? rawBytes.Dequeue() : fill;
      });

      ProcessingHelper.ProcessMono(img.Planes[1], (b) =>
      {
        return rawBytes.Any() ? rawBytes.Dequeue() : fill;
      });

      ProcessingHelper.ProcessMono(img.Planes[2], (b) =>
      {
        return rawBytes.Any() ? rawBytes.Dequeue() : fill;
      });
    }
  }
}