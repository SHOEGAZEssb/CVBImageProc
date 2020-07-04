using System;

namespace CVBImageProc.Processing.ValueProvider
{
  /// <summary>
  /// ViewModel for <see cref="IValueProvider{T}"/>s.
  /// </summary>
  /// <typeparam name="T">Type of value to provide.</typeparam>
  class ValueProviderViewModel<T> : SettingsViewModelBase where T : struct
  {
    #region Properties

    /// <summary>
    /// The configured value to provide in
    /// normal mode.
    /// </summary>
    public T FixedValue
    {
      get => _provider.FixedValue;
      set
      {
        if (!FixedValue.Equals(value))
        {
          _provider.FixedValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// The minimum possible value.
    /// </summary>
    public T MinValue => _provider.MinValue;

    /// <summary>
    /// The maximum possible value.
    /// </summary>
    public T MaxValue => _provider.MaxValue;

    /// <summary>
    /// If true, randomizes the values to provide.
    /// </summary>
    public bool Randomize
    {
      get => _provider.Randomize;
      set
      {
        if (Randomize != value)
        {
          _provider.Randomize = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// The minimum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    public T MinRandomValue
    {
      get => _provider.MinRandomValue;
      set
      {
        if (!MinRandomValue.Equals(value))
        {
          _provider.MinRandomValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    /// <summary>
    /// The maximum value to use in
    /// <see cref="Randomize"/> mode.
    /// </summary>
    public T MaxRandomValue
    {
      get => _provider.MaxRandomValue;
      set
      {
        if (!MaxRandomValue.Equals(value))
        {
          _provider.MaxRandomValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    #endregion Properties

    #region Member

    /// <summary>
    /// The provider.
    /// </summary>
    private readonly IValueProvider<T> _provider;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="provider">The provider.</param>
    public ValueProviderViewModel(IValueProvider<T> provider)
    {
      _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    #endregion Construction
  }
}