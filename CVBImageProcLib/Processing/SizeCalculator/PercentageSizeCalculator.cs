using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing.SizeCalculator
{
	/// <summary>
	/// Size calculator based on percentage.
	/// </summary>
	[DataContract]
	[DisplayName("Percentage")]
	public sealed class PercentageSizeCalculator : ISizeCalculator
	{
		#region Properties

		/// <summary>
		/// The percentage to use of the input size.
		/// </summary>
		public double Percentage
		{
			get => _percentage;
			set
			{
				if (value < 0.0)
					value = 0.0;

				_percentage = value;
			}
		}
		[DataMember]
		private double _percentage = 1.0;

		#endregion Properties

		#region ISizeCalculator Implementation

		/// <summary>
		/// Gets the calculated size for
		/// the given <paramref name="img"/>.
		/// </summary>
		/// <param name="img">Img to calculate size for.</param>
		/// <returns>Calculated size.</returns>
		public Size2D GetCalculatedSize(Image img)
		{
			ArgumentNullException.ThrowIfNull(img);

			int width = (int)(img.Width * Percentage);
			int height = (int)(img.Height * Percentage);

			if (width < 1)
				width = 1;
			if (height < 1)
				height = 1;

			return new Size2D(width, height);
		}

		#endregion ISizeCalculator Implementation
	}
}