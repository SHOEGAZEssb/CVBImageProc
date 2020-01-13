using CVBImageProc.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Threading.Tasks;

namespace CVBImageProc
{
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
}