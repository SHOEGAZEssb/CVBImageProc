﻿using CVBImageProcLib.Processing.Filter;
using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVBImageProcLib
{
	/// <summary>
	/// Extensions for the <see cref="ICanProcessIndividualRegions"/> interface.
	/// </summary>
	internal static class ICanProcessIndividualRegionsExtensions
	{
		/// <summary>
		/// Calculates the <see cref="ProcessingBounds"/>
		/// for the <paramref name="proc"/> and the given <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="proc">Processor whose settings to use for bound calculation.</param>
		/// <param name="inputImage">Image to use for bound calculation.</param>
		/// <returns>Calculated bounds.</returns>
		public static ProcessingBounds GetProcessingBounds(this ICanProcessIndividualRegions proc, Image inputImage)
		{
			ArgumentNullException.ThrowIfNull(inputImage);

			return proc.UseAOI ? new ProcessingBounds(proc.AOI) : new ProcessingBounds(inputImage.Bounds);
		}
	}

	/// <summary>
	/// Extensions for the <see cref="KernelSize"/> enum.
	/// </summary>
	internal static class KernelSizeExtensions
	{
		/// <summary>
		/// Gets the number representing the given <paramref name="kernel"/>.
		/// </summary>
		/// <param name="kernel">Size to get number representation for.</param>
		/// <returns>Number representation for the given <paramref name="kernel"/>.</returns>
		public static int GetKernelNumber(this KernelSize kernel)
		{
			return kernel switch
			{
				KernelSize.ThreeByThree => 3,
				KernelSize.FiveByFive => 5,
				KernelSize.SevenBySeven => 7,
				_ => throw new ArgumentException("Unknown kernel size", nameof(kernel)),
			};
		}
	}

	/// <summary>
	/// Extensions for a <see cref="Image"/>.
	/// </summary>
	public static class ImageExtensions
	{
		/// <summary>
		/// Gets all pixel values in the image.
		/// Order is plane order. (eg. RGB).
		/// </summary>
		/// <param name="img">Image to get pixel values of.</param>
		/// <returns>Pixel values.</returns>
		public static IEnumerable<byte> GetPixels(this Image img)
		{
			ArgumentNullException.ThrowIfNull(img);

			var pixels = Enumerable.Empty<byte>();
			foreach (var plane in img.Planes)
				pixels = pixels.Concat(plane.GetPixels());

			return pixels;
		}
	}

	/// <summary>
	/// Extensions for a <see cref="ImagePlane"/>.
	/// </summary>
	public static class ImagePlaneExtensions
	{
		/// <summary>
		/// Gets the pixels of the given <paramref name="plane"/>.
		/// </summary>
		/// <param name="plane">Plane to get pixels for.</param>
		/// <returns>Pixels in the plane.</returns>
		public static IEnumerable<byte> GetPixels(this ImagePlane plane)
		{
			return plane.AllPixels.DereferenceAs<byte>();
		}

		/// <summary>
		/// Gets the pixels of the given <paramref name="plane"/>
		/// as a 2D byte array.
		/// </summary>
		/// <param name="plane">Plane to get pixels of.</param>
		/// <returns>Pixels of the given <paramref name="plane"/> as
		/// 2D byte array.</returns>
		public static byte[,] GetPixelsAs2DArray(this ImagePlane plane)
		{
			var inputPixels = plane.GetPixels().ToArray();
			int height = plane.Parent.Height;
			int width = plane.Parent.Width;
			var pixels = new byte[height, width];

			for (int y = 0; y < height; y++)
			{
				int yW = y * width;
				for (int x = 0; x < width; x++)
				{
					pixels[y, x] = inputPixels[yW + x];
				}
			}

			return pixels;
		}
	}
}
