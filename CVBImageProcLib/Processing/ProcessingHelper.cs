using CVBImageProcLib.Processing.Filter;
using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Helper class for pixel access and processing.
  /// </summary>
  static class ProcessingHelper
  {
    #region ProcessMono

    #region ProcessMono B+FC

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// with the given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, processes
    /// it and returns a byte.</param>
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessMono(ImagePlane plane, Func<byte, byte> processorFunc, PixelFilterChain filterChain)
    {
      if (filterChain == null || !filterChain.HasActiveFilter)
        ProcessMono(plane, processorFunc);
      else
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
    public static void ProcessMono(ImagePlane plane, ProcessingBounds bounds, Func<byte, byte> processorFunc, PixelFilterChain filterChain)
    {
      if (filterChain == null || !filterChain.HasActiveFilter)
      {
        ProcessMono(plane, bounds, processorFunc);
        return;
      }
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          var pBase = (byte*)data.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = pBase + yInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              if (filterChain.Check(*pPixel, y * boundsY + x))
                *pPixel = processorFunc.Invoke(*pPixel);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    #endregion ProcessMono B+FC

    #region ProcessMono B

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// with the given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, processes
    /// it and returns a byte.</param>
    public static void ProcessMono(ImagePlane plane, Func<byte, byte> processorFunc)
    {
      ProcessMono(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc);
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
    public static void ProcessMono(ImagePlane plane, ProcessingBounds bounds, Func<byte, byte> processorFunc)
    {
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          var pBase = (byte*)data.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = pBase + yInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              *pPixel = processorFunc.Invoke(*pPixel);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    #endregion ProcessMono B

    #region ProcessMono BII+FC

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// with the given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, y and x, processes
    /// it and returns a byte.</param>
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessMono(ImagePlane plane, Func<byte, int, int, byte> processorFunc, PixelFilterChain filterChain)
    {
      if (filterChain == null || !filterChain.HasActiveFilter)
        ProcessMono(plane, processorFunc);
      else
        ProcessMono(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc, filterChain);
    }

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// in the given <paramref name="bounds"/> with the
    /// given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="bounds">Bounds defining which pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, x and y, processes
    /// it and returns a byte.</param>
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessMono(ImagePlane plane, ProcessingBounds bounds, Func<byte, int, int, byte> processorFunc, PixelFilterChain filterChain)
    {
      if (filterChain == null || !filterChain.HasActiveFilter)
      {
        ProcessMono(plane, bounds, processorFunc);
        return;
      }
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          var pBase = (byte*)data.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = pBase + yInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              if (filterChain.Check(*pPixel, y * boundsY + x))
                *pPixel = processorFunc.Invoke(*pPixel, y, x);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    #endregion ProcessMono BII+FC

    #region ProcessMono BII

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// with the given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, y and x, processes
    /// it and returns a byte.</param>
    public static void ProcessMono(ImagePlane plane, Func<byte, int, int, byte> processorFunc)
    {
      ProcessMono(plane, new ProcessingBounds(plane.Parent.Bounds), processorFunc);
    }

    /// <summary>
    /// Processes the pixels of the given <paramref name="plane"/>
    /// in the given <paramref name="bounds"/> with the
    /// given <paramref name="processorFunc"/>.
    /// </summary>
    /// <param name="plane">The plane whose pixels to process.</param>
    /// <param name="bounds">Bounds defining which pixels to process.</param>
    /// <param name="processorFunc">Func that takes a byte, x and y, processes
    /// it and returns a byte.</param>
    public static void ProcessMono(ImagePlane plane, ProcessingBounds bounds, Func<byte, int, int, byte> processorFunc)
    {
      if (processorFunc == null)
        throw new ArgumentNullException(nameof(processorFunc));

      if (plane.TryGetLinearAccess(out LinearAccessData data))
      {
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;
        unsafe
        {
          var pBase = (byte*)data.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* pLine = pBase + yInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* pPixel = pLine + xInc * x;
              *pPixel = processorFunc.Invoke(*pPixel, y, x);
            }
          }
        }
      }
      else
        throw new ArgumentException("Plane could not be accessed linear", nameof(plane));
    }

    #endregion ProcessMono BII

    #endregion ProcessMono

    #region ProcessMonoKernel

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
        var yInc = (int)data.YInc;
        var xInc = (int)data.XInc;
        var newYInc = (int)newData.YInc;
        var newXInc = (int)newData.XInc;

        int boundHeight = plane.Parent.Height - 1;
        int boundWidth = plane.Parent.Width - 1;
        int boundsY = bounds.StartY + bounds.Height;
        int boundsX = bounds.StartX + bounds.Width;

        int kernelSize = kernel.GetKernelNumber();
        int kernelArrSize = kernelSize * kernelSize;
        var kernelFac = (int)System.Math.Floor(kernelSize / 2.0);
        int kernelCounter = -1;

        unsafe
        {
          var pBase = (byte*)data.BasePtr;
          var pBaseNew = (byte*)newData.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            var kernelValues = new byte?[kernelArrSize];
            var pLine = pBase + y * yInc;
            int newLineInc = newYInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              var pMiddle = pLine + xInc * x;
              for (int kRow = -kernelFac; kRow <= kernelFac; kRow++)
              {
                byte* pKLine = pMiddle + kRow * yInc;
                int yKRow = y + kRow;
                for (int kColumn = -kernelFac; kColumn <= kernelFac; kColumn++)
                {
                  kernelCounter++;
                  int xKColumn = x + kColumn;
                  if (yKRow < 0 || yKRow > boundHeight || xKColumn < 0 || xKColumn > boundWidth)
                    continue;

                  byte* pPixel = pKLine + kColumn * xInc;
                  if (filterChain?.Check(*pPixel, y * boundsY + x) ?? true)
                    kernelValues[kernelCounter] = *pPixel;
                }
              }

              if (kernelValues.Any(b => b.HasValue))
              {
                var pTargetLine = pBaseNew + newLineInc;
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

    #endregion ProcessMonoKernel

    #region ProcessRGB

    #region ProcessRGB BBB+FC

    /// <summary>
    /// Processes the given rgb <paramref name="img"/> with
    /// the given <paramref name="processingFunc"/>.
    /// </summary>
    /// <param name="img">Image to process.</param>
    /// <param name="processingFunc">Processing function to process
    /// the <paramref name="img"/> with.</param>
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessRGB(Image img, Func<RGBPixel, RGBPixel> processingFunc, PixelFilterChain filterChain)
    {
      if (filterChain == null || !filterChain.HasActiveFilter)
        ProcessRGB(img, processingFunc);
      else
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
    public static void ProcessRGB(Image img, Func<RGBPixel, RGBPixel> processingFunc, ProcessingBounds bounds, PixelFilterChain filterChain)
    {
      if(filterChain == null || !filterChain.HasActiveFilter)
      {
        ProcessRGB(img, processingFunc, bounds);
        return;
      }
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
        var rYInc = (int)rData.YInc;
        var gYInc = (int)gData.YInc;
        var bYInc = (int)bData.YInc;
        var rXInc = (int)rData.XInc;
        var gXInc = (int)gData.XInc;
        var bXInc = (int)bData.XInc;

        unsafe
        {
          var pBaseR = (byte*)rData.BasePtr;
          var pBaseG = (byte*)gData.BasePtr;
          var pBaseB = (byte*)bData.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* rLine = pBaseR + rYInc * y;
            byte* gLine = pBaseG + gYInc * y;
            byte* bLine = pBaseB + bYInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* rPixel = rLine + rXInc * x;
              byte* gPixel = gLine + gXInc * x;
              byte* bPixel = bLine + bXInc * x;

              if (filterChain.Check(*rPixel, *gPixel, *bPixel, y * boundsY + x))
              {
                var result = processingFunc.Invoke(new RGBPixel(*rPixel, *gPixel, *bPixel));
                *rPixel = result.R;
                *gPixel = result.G;
                *bPixel = result.B;
              }
            }
          }
        }
      }
      else
        throw new ArgumentException("Image could not be accessed linear", nameof(img));
    }

    #endregion ProcessRGB BBB+FC

    #region ProcessRGB BBB

    /// <summary>
    /// Processes the given rgb <paramref name="img"/> with
    /// the given <paramref name="processingFunc"/>.
    /// </summary>
    /// <param name="img">Image to process.</param>
    /// <param name="processingFunc">Processing function to process
    /// the <paramref name="img"/> with.</param>
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessRGB(Image img, Func<RGBPixel, RGBPixel> processingFunc)
    {
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
    /// <param name="filterChain">Optional filter chain.</param>
    public static void ProcessRGB(Image img, Func<RGBPixel, RGBPixel> processingFunc, ProcessingBounds bounds)
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
        var rYInc = (int)rData.YInc;
        var gYInc = (int)gData.YInc;
        var bYInc = (int)bData.YInc;
        var rXInc = (int)rData.XInc;
        var gXInc = (int)gData.XInc;
        var bXInc = (int)bData.XInc;

        unsafe
        {
          var pBaseR = (byte*)rData.BasePtr;
          var pBaseG = (byte*)gData.BasePtr;
          var pBaseB = (byte*)bData.BasePtr;

          for (int y = bounds.StartY; y < boundsY; y++)
          {
            byte* rLine = pBaseR + rYInc * y;
            byte* gLine = pBaseG + gYInc * y;
            byte* bLine = pBaseB + bYInc * y;

            for (int x = bounds.StartX; x < boundsX; x++)
            {
              byte* rPixel = rLine + rXInc * x;
              byte* gPixel = gLine + gXInc * x;
              byte* bPixel = bLine + bXInc * x;

              var result = processingFunc.Invoke(new RGBPixel(*rPixel, *gPixel, *bPixel));
              *rPixel = result.R;
              *gPixel = result.G;
              *bPixel = result.B;
            }
          }
        }
      }
      else
        throw new ArgumentException("Image could not be accessed linear", nameof(img));
    }

    #endregion ProcessRGB BBB

    #endregion ProcessRGB

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