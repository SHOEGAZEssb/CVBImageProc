using CVBImageProcLib.Processing.PixelFilter;
using CVBImageProcLib.Processing.ValueProvider;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// ViewModel that replaces certain pixel values.
	/// </summary>
	[DataContract]
	public sealed class Replace : FullProcessorBase
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public override string Name => "Replace";

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
			ProcessingHelper.ProcessMonoParallel(plane, bounds, (b) =>
			{
				return ValueProvider.Provide();
			}, PixelFilter);
		}

		#endregion IProcessor Implementation

		#region Properties

		/// <summary>
		/// Value to use when replacing.
		/// </summary>
		[DataMember]
		public ByteValueProvider ValueProvider { get; set; } = new ByteValueProvider(0, 255);

		#endregion Properties
	}
}