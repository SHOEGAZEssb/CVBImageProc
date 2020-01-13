using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing
{
  static class ProcessingHelper
  {
    public static void Process(ImagePlane plane, Func<byte, byte> processorFunc)
    {
      Process(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc);
    }

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