using CVBImageProcLib.Processing;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="PixelShift"/> processor.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="processor">The processor.</param>
	/// <param name="isActive">Default active state.</param>
	internal sealed class PixelShiftViewModel(PixelShift processor, bool isActive) : FullProcessorViewModelBase(processor, isActive)
	{
		#region Properties

		/// <summary>
		/// The shift in X-direction.
		/// </summary>
		public int ShiftX
		{
			get => _processor.ShiftX;
			set
			{
				if (ShiftX != value)
				{
					_processor.ShiftX = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		/// <summary>
		/// The shift in Y-direction.
		/// </summary>
		public int ShiftY
		{
			get => _processor.ShiftY;
			set
			{
				if (ShiftY != value)
				{
					_processor.ShiftY = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		/// <summary>
		/// Gets if pixels should "wrap" around
		/// back to the beginning of the image.
		/// </summary>
		public bool Wrap
		{
			get => _processor.Wrap;
			set
			{
				if (Wrap != value)
				{
					_processor.Wrap = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		/// <summary>
		/// Gets if the processed plane should be
		/// initialized with the<see cref="FillValue"/>.
		/// If not the original plane is used.
		/// </summary>
		public bool UseFillValue
		{
			get => _processor.UseFillValue;
			set
			{
				if (UseFillValue != value)
				{
					_processor.UseFillValue = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		/// <summary>
		/// Value to fill empty pixels with when
		/// <see cref="UseFillValue"/> is true.
		/// </summary>
		public byte FillValue
		{
			get => _processor.FillValue;
			set
			{
				if (FillValue != value)
				{
					_processor.FillValue = value;
					NotifyOfPropertyChange();
					OnSettingsChanged();
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly PixelShift _processor = processor;

		#endregion Member
		#region Construction

		#endregion Construction
	}
}