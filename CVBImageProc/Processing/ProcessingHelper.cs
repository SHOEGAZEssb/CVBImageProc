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
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessMono(ImagePlane plane, Func<byte, byte> processorFunc, PixelFilterChain filterChain = null)
    {
      ProcessMono(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc, filterChain);
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
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessMono(ImagePlane plane, ProcessingBounds bounds, Func<byte, byte> processorFunc, PixelFilterChain filterChain = null)
    {
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = (byte*)(data.BasePtr + (int)data.YInc * y);

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + (int)data.XInc * x;
              if(filterChain?.Check(*pPixel, y * boundsY + x) ?? true)
                *pPixel = processorFunc.Invoke(*pPixel);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    /// <summary>
    /// Processes the given rgb <paramref name="img"/> with
    /// the given <paramref name="processingFunc"/>.
    /// </summary>
    /// <param name="img">Image to process.</param>
    /// <param name="processingFunc">Processing function to process
    /// the <paramref name="img"/> with.</param>
    public static void ProcessRGB(Image img, Func<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> processingFunc)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));
      if (img.Planes.Count != 3)
        throw new ArgumentException("Image is no rgb image", nameof(img));

      ProcessRGB(img, processingFunc, new ProcessingBounds(img.Bounds));
    }

    /// <summary>
    /// Processes the given rgb <paramref name="img"/> in the
    /// given <paramref name="bounds"/> with
    /// the given <paramref name="processingFunc"/>.
    /// </summary>
    /// <param name="img">Image to process.</param>
    /// <param name="processingFunc">Processing function to process
    /// the <paramref name="img"/> with.</param>
    /// <param name="bounds">Bounds defining which pixels to process.</param>
    public static void ProcessRGB(Image img, Func<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> processingFunc, ProcessingBounds bounds)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));
      if (img.Planes.Count != 3)
        throw new ArgumentException("Image is no rgb image", nameof(img));
      if (processingFunc == null)
        throw new ArgumentNullException(nameof(processingFunc));

      if (img.Planes[0].TryGetLinearAccess(out LinearAccessData rData) &&
          img.Planes[1].TryGetLinearAccess(out LinearAccessData gData) &&
          img.Planes[2].TryGetLinearAccess(out LinearAccessData bData))
      {

        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* rLine = (byte*)(rData.BasePtr + (int)rData.YInc * y);
            byte* gLine = (byte*)(gData.BasePtr + (int)gData.YInc * y);
            byte* bLine = (byte*)(bData.BasePtr + (int)bData.YInc * y);

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* rPixel = rLine + (int)rData.XInc * x;
              byte* gPixel = gLine + (int)gData.XInc * x;
              byte* bPixel = bLine + (int)bData.XInc * x;

              var result = processingFunc.Invoke(new Tuple<byte, byte, byte>(*rPixel, *gPixel, *bPixel));
              *rPixel = result.Item1;
              *gPixel = result.Item2;
              *bPixel = result.Item3;
            }
          }
        }
      }
      else
        throw new ArgumentException("Image could not be accessed linear", nameof(img));
    }
  }
}