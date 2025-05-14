using CVBImageProc.MVVM;
using CVBImageProcLib.Processing;
using System;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// ViewModel for a <see cref="RGBFactorPreset"/>.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="name">The name of the preset.</param>
	/// <param name="preset">The preset.</param>
	internal sealed class RGBFactorPresetViewModel(string name, RGBFactorPreset preset) : ViewModelBase
	{
		#region Properties

		/// <summary>
		/// Name of the preset.
		/// </summary>
		public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

		/// <summary>
		/// The preset.
		/// </summary>
		public RGBFactorPreset Preset { get; } = preset;

		#endregion Properties
		#region Construction

		#endregion Construction
	}
}