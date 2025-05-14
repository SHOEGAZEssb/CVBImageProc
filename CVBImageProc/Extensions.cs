using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVBImageProc
{
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
	}

	/// <summary>
	/// Extensions for the <see cref="Task"/> class.
	/// </summary>
	internal static class TaskExtensions
	{
#pragma warning disable IDE0060 // Remove unused parameter
		/// <summary>
		/// Explicitly states that we don't
		/// want to do anything with the <paramref name="task"/>.
		/// </summary>
		/// <param name="task">Task to forget.</param>
		public static void Forget(this Task task)
#pragma warning restore IDE0060 // Remove unused parameter
		{ }
	}
}