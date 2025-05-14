using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Mode to use while sorting.
	/// </summary>
	public enum SortMode
	{
		/// <summary>
		/// Pixels will be sorted in ascending order.
		/// </summary>
		Ascending,

		/// <summary>
		/// Pixels will be sorted in descending order.
		/// </summary>
		Descending
	}

	/// <summary>
	/// Processor that sorts an image plane.
	/// </summary>
	[DataContract]
	public sealed class Sort : FullProcessorBase
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Sort";

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
			byte[] sortedBytes;

			unsafe
			{
				if (UseAOI)
				{
					if (Mode == SortMode.Ascending)
						sortedBytes = [.. plane.GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderBy(i => i)];
					else
						sortedBytes = [.. plane.GetAllPixelsIn(AOI).Select(p => *(byte*)p).OrderByDescending(i => i)];
				}
				else
				{
					if (Mode == SortMode.Ascending)
						sortedBytes = [.. plane.AllPixels.Select(p => *(byte*)p).OrderBy(i => i)];
					else
						sortedBytes = [.. plane.AllPixels.Select(p => *(byte*)p).OrderByDescending(i => i)];
				}
			}

			var byteQueue = new ConcurrentQueue<byte>(sortedBytes);
			ProcessingHelper.ProcessMonoParallel(plane, bounds, (b) =>
			{
				byteQueue.TryDequeue(out byte value);
				return value;
			}, PixelFilter);
		}

		#endregion IProcessor Implementation

		#region Properties

		/// <summary>
		/// Sorting mode.
		/// </summary>
		[DataMember]
		public SortMode Mode { get; set; }

		#endregion Properties
	}
}