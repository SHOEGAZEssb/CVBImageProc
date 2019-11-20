using System;

namespace CVBImageProc.MVVM
{
  /// <summary>
  /// ViewModel for a type.
  /// </summary>
  class TypeViewModel : ViewModelBase
  {
    #region Properties

    /// <summary>
    /// Name of the type.
    /// </summary>
    public string Name => _type.Name;

    #endregion Properties

    #region Member

    /// <summary>
    /// Wrapped type.k
    /// </summary>
    private readonly Type _type;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="type">Type to wrap.</param>
    public TypeViewModel(Type type)
    {
      _type = type ?? throw new ArgumentNullException(nameof(type));
    }

    #endregion Construction

    /// <summary>
    /// Instantiates an object of the wrapped type.
    /// </summary>
    /// <returns></returns>
    public object Instanciate()
    {
      return Activator.CreateInstance(_type);
    }
  }
}