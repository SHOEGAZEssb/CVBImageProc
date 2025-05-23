﻿using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.Filter
{
	/// <summary>
	/// Filter processor with custom weights.
	/// </summary>
	[DataContract]
	[CustomFilterSettings]
	public sealed class Custom : WeightedFilterBase
	{
		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Custom";

		/// <summary>
		/// Processes the <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage">Image to process.</param>
		/// <returns>Processed image.</returns>
		public override Image Process(Image inputImage)
		{
			ArgumentNullException.ThrowIfNull(inputImage);

			var bounds = this.GetProcessingBounds(inputImage);
			int weightSum = Weights.Sum();
			if (ProcessAllPlanes)
			{
				foreach (var plane in inputImage.Planes)
					ProcessPlane(plane, bounds, weightSum);
			}
			else
				ProcessPlane(inputImage.Planes[PlaneIndex], bounds, weightSum);

			return inputImage;
		}

		private void ProcessPlane(ImagePlane plane, ProcessingBounds bounds, int weightSum)
		{
			var outputPlane = ProcessingHelper.ProcessMonoKernelParallel(plane, (kl) =>
			{
				return ApplyWeights(kl, Weights, weightSum);
			}, KernelSize, bounds, PixelFilter);

			outputPlane.CopyTo(plane.Parent.Planes[plane.Plane]);
		}

		#region Properties

		/// <summary>
		/// Kernel size to use.
		/// </summary>
		[IgnoreDataMember]
		public override KernelSize KernelSize
		{
			get => _kernelSize;
			set
			{
				if (KernelSize != value)
				{
					_kernelSize = value;
					CreateWeights();
				}
			}
		}
		[DataMember]
		private KernelSize _kernelSize;

		/// <summary>
		/// The custom weights.
		/// </summary>
		[DataMember]
		public int[] Weights { get; set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		public Custom()
		{
			CreateWeights();
		}

		#endregion Construction

		/// <summary>
		/// Creates the <see cref="Weights"/>
		/// based on the <see cref="KernelSize"/>.
		/// </summary>
		private void CreateWeights()
		{
			int num = KernelSize.GetKernelNumber();
			Weights = new int[num * num];
		}
	}
}