using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
	/// <summary>
	/// Processor for converting an RGB image
	/// to a mono image.
	/// </summary>
	[DataContract]
	[DisplayName("RGB To Mono")]
	public sealed class RGBToMono : IProcessor
	{
		#region IProcessor Implementation

		/// <summary>
		/// Name of the processor.
		/// </summary>
		public string Name => "RGB To Mono";

		/// <summary>
		/// Processes the <paramref name="inputImage"/>.
		/// </summary>
		/// <param name="inputImage">Image to process.</param>
		/// <returns>Processed image.</returns>
		public Image Process(Image inputImage)
		{
			ArgumentNullException.ThrowIfNull(inputImage);
			if (inputImage.Planes.Count < 3)
				throw new ArgumentException($"Input image is not compatible with {Name}. (Too few planes)", nameof(inputImage));

			Image monoImage = null;
			if (inputImage.Planes[0].TryGetLinearAccess(out LinearAccessData rData) &&
			   inputImage.Planes[1].TryGetLinearAccess(out LinearAccessData gData) &&
			   inputImage.Planes[2].TryGetLinearAccess(out LinearAccessData bData))
			{
				monoImage = new Image(inputImage.Size);
				var mData = monoImage.Planes[0].GetLinearAccess();

				int rYInc = (int)rData.YInc;
				int gYInc = (int)gData.YInc;
				int bYInc = (int)bData.YInc;
				int rXInc = (int)rData.XInc;
				int gXInc = (int)gData.XInc;
				int bXInc = (int)bData.XInc;
				int mXInc = (int)mData.XInc;
				int mYInc = (int)mData.YInc;

				int height = inputImage.Height;
				int width = inputImage.Width;

				unsafe
				{
					byte* pBaseR = (byte*)rData.BasePtr;
					byte* pBaseG = (byte*)gData.BasePtr;
					byte* pBaseB = (byte*)bData.BasePtr;
					byte* pBaseM = (byte*)mData.BasePtr;

					for (int y = 0; y < height; y++)
					{
						byte* rLine = pBaseR + rYInc * y;
						byte* gLine = pBaseG + gYInc * y;
						byte* bLine = pBaseB + bYInc * y;
						byte* mLine = pBaseM + mYInc * y;

						for (int x = 0; x < width; x++)
						{
							byte* rPixel = rLine + rXInc * x;
							byte* gPixel = gLine + gXInc * x;
							byte* bPixel = bLine + bXInc * x;
							byte* mPixel = mLine + mXInc * x;

							*mPixel = (byte)((*rPixel * FactorR) + (*gPixel * FactorG) + (*bPixel * FactorB));
						}
					}
				}
			}
			else
				throw new ArgumentException($"Input image could not be accessed linearly", nameof(inputImage));

			return monoImage;
		}

		#endregion IProcessor Implementation

		#region Consts

		/// <summary>
		/// Red conversion factor.
		/// </summary>
		public const double DEFAULTFACTORRED = 0.2125;

		/// <summary>
		/// Green conversion factor.
		/// </summary>
		public const double DEFAULTFACTORGREEN = 0.7154;

		/// <summary>
		/// Blue conversion factor.
		/// </summary>
		public const double DEFAULTFACTORBLUE = 0.0721;

		#endregion Consts

		#region Properties

		/// <summary>
		/// Factor of the R pixel.
		/// </summary>
		[DataMember]
		public double FactorR { get; set; } = DEFAULTFACTORRED;

		/// <summary>
		/// Factor of the G pixel.
		/// </summary>
		[DataMember]
		public double FactorG { get; set; } = DEFAULTFACTORGREEN;

		/// <summary>
		/// Factor of the B pixel.
		/// </summary>
		[DataMember]
		public double FactorB { get; set; } = DEFAULTFACTORBLUE;

		#endregion Properties
	}
}
