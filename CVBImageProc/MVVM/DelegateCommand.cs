﻿using System;
using System.Windows.Input;

namespace CVBImageProc.MVVM
{
	/// <summary>
	/// Command that can be executed via the UI.
	/// </summary>
	/// <remarks>
	/// Constructor.
	/// </remarks>
	/// <param name="execute">Delegate to execute.</param>
	/// <param name="canExecute">Predicate to check if the
	/// command can be executed.</param>
	public sealed class DelegateCommand(Action<object> execute, Predicate<object> canExecute = null) : ICommand
	{
		#region ICommand Implementation

		/// <summary>
		/// Event that is fired when the can execute state
		/// of the command might have changed.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Checks if the command can be executed.
		/// </summary>
		/// <param name="parameter">Parameter for the check.</param>
		/// <returns>true if the command can be executed, otherwise false.</returns>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameter">Parameter for the command.</param>
		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		#endregion ICommand Implementation

		#region Member

		/// <summary>
		/// Delegate to execute.
		/// </summary>
		private readonly Predicate<object> _canExecute = canExecute;

		/// <summary>
		/// Predicate to check if the
		/// command can be executed.
		/// </summary>
		private readonly Action<object> _execute = execute;

		#endregion Member
		#region Construction

		#endregion Construction

		/// <summary>
		/// Fires the <see cref="CanExecuteChanged"/> event.
		/// </summary>
		public void OnCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}