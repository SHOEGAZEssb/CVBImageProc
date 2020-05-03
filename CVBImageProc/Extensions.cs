﻿using CVBImageProc.Processing;
using CVBImageProc.Processing.PixelFilter;
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
    /// </summary>
    /// <param name="img">Image to get pixel values of.</param>
    /// <returns>Pixel values.</returns>
    public static IEnumerable<byte> GetPixels(this Image img)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));

      var data = img.Planes.Select(p => p.GetLinearAccess()).ToArray();

      var pixels = new byte[(img.Width * img.Height) * img.Planes.Count];
      int curPixel = 0;

      for (int i = 0; i < img.Planes.Count; i++)
      {
        unsafe
        {
          for (int y = 0; y < img.Height; y++)
          {
            byte* pLine = (byte*)(data[i].BasePtr + (int)data[i].YInc * y);

            for (int x = 0; x < img.Width; x++)
            {
              pixels[curPixel++] = *(pLine + (int)data[i].XInc * x);
            }
          }
        }
      }

      return pixels;
    }
  }

  /// <summary>
  /// Extensions for the <see cref="Task"/> class.
  /// </summary>
  static class TaskExtensions
  {
#pragma warning disable IDE0060 // Remove unused parameter
                               /// <summary>
                               /// Explicitly states that we don't
                               /// want to do anything with the <paramref name="task"/>.
                               /// </summary>
                               /// <param name="task">task to forget.</param>
    public static void Forget(this Task task)
#pragma warning restore IDE0060 // Remove unused parameter
    { }
  }

  /// <summary>
  /// Extensions for the <see cref="ICanProcessIndividualRegions"/> interface.
  /// </summary>
  static class ICanProcessIndividualRegionsExtensions
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
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      return proc.UseAOI ? new ProcessingBounds(proc.AOI) : new ProcessingBounds(inputImage.Bounds);
    }
  }

  static class KernelSizeExtensions
  {
    public static int GetKernelNumber(this KernelSize kernel)
    {
      switch(kernel)
      {
        case KernelSize.ThreeByThree:
          return 3;
        case KernelSize.FiveByFive:
          return 5;
        default:
          throw new ArgumentException("Unknown kernel size", nameof(kernel));
      }
    }
  }
}