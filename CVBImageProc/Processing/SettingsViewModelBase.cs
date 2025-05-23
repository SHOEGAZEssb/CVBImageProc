﻿using CVBImageProc.MVVM;
using System;

namespace CVBImageProc.Processing
{
	/// <summary>
	/// Base class for ViewModels that implement <see cref="IHasSettings"/>.
	/// </summary>
	internal abstract class SettingsViewModelBase : ViewModelBase, IHasSettings
	{
		#region IHasSettings Implementation

		/// <summary>
		/// Event that is fired when one of
		/// the settings changed.
		/// </summary>
		public event EventHandler SettingsChanged;

		#endregion IHasSettings Implementation

		/// <summary>
		/// Fires the <see cref="SettingsChanged"/> event.
		/// </summary>
		protected void OnSettingsChanged()
		{
			SettingsChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}