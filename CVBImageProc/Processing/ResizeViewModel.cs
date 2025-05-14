using CVBImageProc.MVVM;
using CVBImageProc.Processing.SizeCalculator;
using CVBImageProcLib.Processing;
using CVBImageProcLib.Processing.SizeCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for the <see cref="Resize"/> processor.
	/// </summary>
	internal sealed class ResizeViewModel : ProcessorViewModel, IHasSettings
	{
		#region Properties

		/// <summary>
		/// Event that is fired when one of
		/// the settings changed.
		/// </summary>
		public event EventHandler SettingsChanged;

		/// <summary>
		/// Gets the available size calculator types.
		/// </summary>
		public static IEnumerable<TypeViewModel> AvailableSizeCalculators { get; }

		/// <summary>
		/// The currently selected size calculator type.
		/// </summary>
		public TypeViewModel SelectedSizeCalculatorType
		{
			get => _selectedSizeCalculatorType;
			set
			{
				if (SelectedSizeCalculatorType != value)
				{
					_selectedSizeCalculatorType = value;
					NotifyOfPropertyChange();
					SelectedSizeCalculator = MakeSizeCalculatorViewModel((ISizeCalculator)SelectedSizeCalculatorType.Instanciate());
				}
			}
		}
		private TypeViewModel _selectedSizeCalculatorType;

		/// <summary>
		/// The currently selected size calculator.
		/// </summary>
		public SizeCalculatorViewModelBase SelectedSizeCalculator
		{
			get => _selectedSizeCalculator;
			set
			{
				if (SelectedSizeCalculator != value)
				{
					if (SelectedSizeCalculator != null)
						SelectedSizeCalculator.SettingsChanged -= SelectedSizeCalculator_SettingsChanged;

					_selectedSizeCalculator = value;
					if (SelectedSizeCalculator != null)
						SelectedSizeCalculator.SettingsChanged += SelectedSizeCalculator_SettingsChanged;
					_processor.SizeCalculator = SelectedSizeCalculator.SizeCalculator;
					NotifyOfPropertyChange();
					SettingsChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		private SizeCalculatorViewModelBase _selectedSizeCalculator;

		/// <summary>
		/// Algorithm to use for scaling.
		/// </summary>
		public ScaleMode Mode
		{
			get => _processor.Mode;
			set
			{
				if (Mode != value)
				{
					_processor.Mode = value;
					NotifyOfPropertyChange();
					SettingsChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		#endregion Properties

		#region Member

		/// <summary>
		/// The processor.
		/// </summary>
		private readonly Resize _processor;

		#endregion Member

		#region Construction

		/// <summary>
		/// Static constructor.
		/// </summary>
		static ResizeViewModel()
		{
			AvailableSizeCalculators = [.. Assembly.GetAssembly(typeof(ISizeCalculator)).GetTypes()
							.Where(mytype => mytype.GetInterfaces().Contains(typeof(ISizeCalculator)) && !mytype.IsInterface && !mytype.IsAbstract)
							.Select(i => new TypeViewModel(i)).OrderBy(t => t.Name)];
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="isActive">Startup IsActive state.</param>
		public ResizeViewModel(Resize processor, bool isActive)
		  : base(processor, isActive)
		{
			_processor = processor;
			_selectedSizeCalculatorType = AvailableSizeCalculators.First(t => t.Type == _processor.SizeCalculator.GetType());
			SelectedSizeCalculator = MakeSizeCalculatorViewModel(_processor.SizeCalculator);
		}

		#endregion Construction

		private static SizeCalculatorViewModelBase MakeSizeCalculatorViewModel(ISizeCalculator sizeCalculator)
		{
			return sizeCalculator switch
			{
				FreeSizeCalculator f => new FreeSizeCalculatorViewModel(f),
				PercentageSizeCalculator p => new PercentageSizeCalculatorViewModel(p),
				_ => throw new ArgumentException("Unknown size calculator", nameof(sizeCalculator)),
			};
		}

		private void SelectedSizeCalculator_SettingsChanged(object sender, EventArgs e)
		{
			SettingsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}