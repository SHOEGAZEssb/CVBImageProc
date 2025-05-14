using CVBImageProcLib.Processing;
using Stemmer.Cvb;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CVBImageProcLib
{
	/// <summary>
	/// How rgb pixels should be read
	/// from the raw bytes.
	/// </summary>
	public enum RGBMode
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
	public static class RawFileImporter
	{
		/// <summary>
		/// Imports the given <paramref name="rawBytes"/> as mono image.
		/// </summary>
		/// <param name="rawBytes">The raw bytes to use for the image.</param>
		/// <param name="imageSize">Size of the resulting image.</param>
		/// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
		/// <returns>Imported mono image.</returns>
		public static Image ImportAsMono(IEnumerable<byte> rawBytes, Size2D imageSize, byte fill)
		{
			ArgumentNullException.ThrowIfNull(rawBytes);

			var byteQueue = new ConcurrentQueue<byte>(rawBytes);
			var img = new Image(imageSize);
			ProcessingHelper.ProcessMonoParallel(img.Planes[0], (b) =>
			{
				return byteQueue.TryDequeue(out var value) ? value : fill;
			});

			return img;
		}

		/// <summary>
		/// Imports the given <paramref name="rawBytes"/> as rgb image.
		/// </summary>
		/// <param name="rawBytes">The raw bytes to use for the image.</param>
		/// <param name="imageSize">Size of the resulting image.</param>
		/// <param name="fill">Pixel value used as a fill when no raw bytes are left.</param>
		/// <param name="rgbMode">Read-in mode of the rgb bytes.</param>
		/// <returns>Imported rgb image.</returns>
		public static Image ImportAsRGB(IEnumerable<byte> rawBytes, Size2D imageSize, byte fill, RGBMode rgbMode)
		{
			ArgumentNullException.ThrowIfNull(rawBytes);

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
			ArgumentNullException.ThrowIfNull(rawBytes);
			ArgumentNullException.ThrowIfNull(img);

			// Convert to thread-safe queue
			var byteQueue = new ConcurrentQueue<byte>(rawBytes);
			ProcessingHelper.ProcessRGBParallel(img, (i) =>
			{
				byte r = byteQueue.TryDequeue(out var rVal) ? rVal : fill;
				byte g = byteQueue.TryDequeue(out var gVal) ? gVal : fill;
				byte b = byteQueue.TryDequeue(out var bVal) ? bVal : fill;
				return new RGBPixel(r, g, b);
			});
		}

		private static void ImportRGBAsRRGGBB(Queue<byte> rawBytes, Image img, byte fill)
		{
			ArgumentNullException.ThrowIfNull(rawBytes);
			ArgumentNullException.ThrowIfNull(img);

			var byteQueue = new ConcurrentQueue<byte>(rawBytes);
			foreach (var plane in img.Planes.Take(3)) // Red, Green, Blue planes
			{
				ProcessingHelper.ProcessMonoParallel(plane, (b) =>
				{
					return byteQueue.TryDequeue(out var value) ? value : fill;
				});
			}
		}

	}
}