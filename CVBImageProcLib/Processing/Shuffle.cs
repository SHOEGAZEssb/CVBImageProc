using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Processor that shuffles an image plane.
	/// </summary>
	[DataContract]
	public sealed class Shuffle : FullProcessorBase
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Shuffle";

		/// <summary>
		/// Processes the <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage">Image to process.</param>
		/// <returns>Processed image.</returns>
		public override Image Process(Image inputImage)
		{
			ArgumentNullException.ThrowIfNull(inputImage);

			var bounds = this.GetProcessingBounds(inputImage);
			if (ProcessAllPlanes)
			{
				foreach (var plane in inputImage.Planes)
					ProcessPlane(plane, bounds);
			}
			else
				ProcessPlane(inputImage.Planes[PlaneIndex], bounds);

			return inputImage;
		}

		private void ProcessPlane(ImagePlane plane, ProcessingBounds bounds)
		{
			var rnd = new Random(DateTime.Now.Ticks.GetHashCode());
			int byteCounter = 0;
			byte[] shuffledBytes;

			unsafe
			{
				if (UseAOI)
					shuffledBytes = [.. plane.GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderBy(i => rnd.Next())];
				else
					shuffledBytes = [.. plane.AllPixels.Select(p => *(byte*)p).OrderBy(i => rnd.Next())];
			}

			ProcessingHelper.ProcessMonoParallel(plane, bounds, (b) =>
			{
				return shuffledBytes[byteCounter++];
			}, PixelFilter);
		}

		#endregion IProcessor Implementation
	}
}