namespace CVBImageProc.Processing.Filter
{
  class KernelValueViewModel : SettingsViewModelBase
  {
    public int Value
    {
      get => _value;
      set
      {
        if(Value != value)
        {
          _value = value;
          OnSettingsChanged();
          NotifyOfPropertyChange();
        }
      }
    }
    private int _value;

    #region Construction

    public KernelValueViewModel(int value)
    {
      Value = value;
    }

    #endregion Construction
  }
}