﻿using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
	/// <summary>
	/// Min filter processor.
	/// </summary>
	[DataContract]
	public sealed class Min : FilterBase
	{
		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Min";

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
			var outputPlane = ProcessingHelper.ProcessMonoKernelParallel(plane, (kl) =>
			{
				return kl.Where(b => b.HasValue).Min(b => b.Value);
			}, KernelSize, bounds, PixelFilter);

			outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
		}
	}
}