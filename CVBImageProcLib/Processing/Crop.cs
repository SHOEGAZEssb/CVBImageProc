﻿using CVBImageProcLib.Processing.PixelFilter;
using Stemmer.Cvb;
using System;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Processor that crops an image.
	/// </summary>
	[DataContract]
	public sealed class Crop : IProcessor, ICanProcessIndividualRegions
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public string Name => "Crop";

		/// <summary>
		/// Applies the gain value
		/// to the given <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage">Image to apply gain to.</param>
		/// <returns>Processed image.</returns>
		public Image Process(Image inputImage)
		{
			ArgumentNullException.ThrowIfNull(inputImage);

			return inputImage.Map(AOI);
		}

		#endregion IProcessor Implementation

		#region ICanProcessIndividualRegions Implementation

		/// <summary>
		/// If true, uses the <see cref="AOI"/>
		/// while processing.
		/// </summary>
		[DataMember]
		public bool UseAOI
		{
			get => true;
			set => throw new InvalidOperationException($"{nameof(UseAOI)} can't be set");
		}

		/// <summary>
		/// The AOI to process.
		/// </summary>
		[DataMember]
		public Rect AOI { get; set; }

		#endregion ICanProcessIndividualRegions Implementation
	}
}