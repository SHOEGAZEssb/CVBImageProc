using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// Helper class for pixel access and processing.
  /// </summary>
  static class ProcessingHelper
  {
    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// with the given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, processes
    /// it and returns a byte.</param>
    public static void Process(ImagePlane plane, Func<byte, byte> processorFunc)
    {
      Process(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc);
    }

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// in the given <paramref name="bounds"/> with the
    /// given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="bounds">Bounds defining which pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, processes
    /// it and returns a byte.</param>
    public static void Process(ImagePlane plane, ProcessingBounds bounds, Func<byte, byte> processorFunc)
    {
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        unsafe
        {
          for (int y = bounds.StartY; y < bounds.StartY + bounds.Height; y++)
          {
            byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

            for (int x = bounds.StartX; x < bounds.StartX + bounds.Width; x++)
            {
              byte* pPixel = pLine + (int)data.XInc * x;
              *pPixel = processorFunc.Invoke(*pPixel);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }
  }
}