﻿using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Inverts an image.
	/// </summary>
	[DataContract]
	public sealed class Invert : FullProcessorBase
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Invert";

		/// <summary>
		/// Inverts the given <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage"></param>
		/// <returns></returns>
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
			ProcessingHelper.ProcessMonoParallel(plane, bounds, (b) =>
			{
				return (byte)(255 - b);
			}, PixelFilter);
		}

		#endregion IProcessor Implementation
	}
}