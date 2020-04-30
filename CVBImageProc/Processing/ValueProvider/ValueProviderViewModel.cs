using CVBImageProc.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProc.Processing.ValueProvider
{
  class ValueProviderViewModel<T> : SettingsViewModelBase where T : struct
  {
    #region Properties

    public T FixedValue
    {
      get => _provider.FixedValue;
      set
      {
        if(!FixedValue.Equals(value))
        {
          _provider.FixedValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    public T MinValue => _provider.MinValue;

    public T MaxValue => _provider.MaxValue;

    public bool Randomize
    {
      get => _provider.Randomize;
      set
      {
        if(Randomize != value)
        {
          _provider.Randomize = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

    public T MinRandomValue
    {
      get => _provider.MinRandomValue;
      set
      {
        if(!MinRandomValue.Equals(value))
        {
          _provider.MinRandomValue = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }

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

    private readonly IValueProvider<T> _provider;

    #endregion Member

    #region Construction

    public ValueProviderViewModel(IValueProvider<T> provider)
    {
      _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    #endregion Construction
  }
}