using CVBImageProc.Processing.Filter;
using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;

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
        int yInc = (int)data.YInc;
        int xInc = (int)data.XInc;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = (byte*)(data.BasePtr + yInc * y);

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              if (filterChain?.Check(*pPixel, y * boundsY + x) ?? true)
                *pPixel = processorFunc.Invoke(*pPixel);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    public static ImagePlane ProcessMonoKernel(ImagePlane plane, Func<byte?[], byte> processingFunc, KernelSize kernel, PixelFilterChain filterChain = null)
    {
      return ProcessMonoKernel(plane, processingFunc, kernel, new ProcessingBounds(plane.Parent.Bounds), filterChain);
    }

    public static ImagePlane ProcessMonoKernel(ImagePlane plane, Func<byte?[], byte> processingFunc, KernelSize kernel, ProcessingBounds bounds, PixelFilterChain filterChain = null)
    {
      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        var newImage = Image.FromPlanes(MappingOption.CopyPixels, plane);
        var newData = newImage.Planes[0].GetLinearAccess();
        int yInc = (int)data.YInc;
        int xInc = (int)data.XInc;
        int newYInc = (int)newData.YInc;
        int newXInc = (int)newData.XInc;

        int boundHeight = plane.Parent.Height - 1;
        int boundWidth = plane.Parent.Width - 1;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;

        int kernelSize = kernel.GetKernelNumber();
        int kernelFac = (int)System.Math.Floor(kernelSize / 2.0);
        int kernelCounter = -1;

        unsafe
        {
          for (int y = bounds.StartY; y < boundsY; y++)
          {
            var kernelValues = new byte?[kernelSize * kernelSize];
            byte* pLine = (byte*)data.BasePtr + y * yInc;
            for (int x = bounds.StartX; x < boundsX; x++)
            {
              var pMiddle = pLine + xInc * x;
              for (int kRow = -kernelFac; kRow <= kernelFac; kRow++)
              {
                byte* pKLine = pMiddle + kRow * yInc;
                for (int kColumn = -kernelFac; kColumn <= kernelFac; kColumn++)
                {
                  kernelCounter++;
                  if (y + kRow < 0 || y + kRow > boundHeight || x + kColumn < 0 || x + kColumn > boundWidth)
                    continue;

                  byte* pPixel = pKLine + kColumn * xInc;
                  if (filterChain?.Check(*pPixel, y * boundsY + x) ?? false)
                    kernelValues[kernelCounter] = *pPixel;
                }
              }

              if (kernelValues.Any(b => b.HasValue))
              {
                var pTargetLine = (byte*)newData.BasePtr + newYInc * y;
                var pTargetPixel = pTargetLine + newXInc * x; // current "middle pixel" in the target image
                *pTargetPixel = processingFunc.Invoke(kernelValues);
              }

              kernelCounter = -1;
            }
          }
        }

        return newImage.Planes[0];
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
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessRGB(Image img, Func<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> processingFunc, PixelFilterChain filterChain = null)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));
      if (img.Planes.Count < 3)
        throw new ArgumentException("Image is no rgb image", nameof(img));

      ProcessRGB(img, processingFunc, new ProcessingBounds(img.Bounds), filterChain);
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
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessRGB(Image img, Func<Tuple<byte, byte, byte>, Tuple<byte, byte, byte>> processingFunc, ProcessingBounds bounds,
                                  PixelFilterChain filterChain = null)
    {
      if (img == null)
        throw new ArgumentNullException(nameof(img));
      if (img.Planes.Count < 3)
        throw new ArgumentException("Image is no rgb image", nameof(img));
      if (processingFunc == null)
        throw new ArgumentNullException(nameof(processingFunc));

      if (img.Planes[0].TryGetLinearAccess(out LinearAccessData rData) &&
          img.Planes[1].TryGetLinearAccess(out LinearAccessData gData) &&
          img.Planes[2].TryGetLinearAccess(out LinearAccessData bData))
      {
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        int rYInc = (int)rData.YInc;
        int gYInc = (int)gData.YInc;
        int bYInc = (int)bData.YInc;
        int rXInc = (int)rData.XInc;
        int gXInc = (int)gData.XInc;
        int bXInc = (int)bData.XInc;
        unsafe
        {
          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* rLine = (byte*)(rData.BasePtr + rYInc * y);
            byte* gLine = (byte*)(gData.BasePtr + gYInc * y);
            byte* bLine = (byte*)(bData.BasePtr + bYInc * y);

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* rPixel = rLine + rXInc * x;
              byte* gPixel = gLine + gXInc * x;
              byte* bPixel = bLine + bXInc * x;

              if (filterChain?.Check(*rPixel, *gPixel, *bPixel, y * boundsY + x) ?? false)
              {
                var result = processingFunc.Invoke(new Tuple<byte, byte, byte>(*rPixel, *gPixel, *bPixel));
                *rPixel = result.Item1;
                *gPixel = result.Item2;
                *bPixel = result.Item3;
              }
            }
          }
        }
      }
      else
        throw new ArgumentException("Image could not be accessed linear", nameof(img));
    }

    public static int ClampPixelValue(int value)
    {
      if (value > 255)
        value = 255;
      else if (value < 0)
        value = 0;
      return value;
    }

    public static int ClampPixelValue(double value)
    {
      if (value > 255)
        value = 255;
      else if (value < 0)
        value = 0;
      return (int)value;
    }
  }
}