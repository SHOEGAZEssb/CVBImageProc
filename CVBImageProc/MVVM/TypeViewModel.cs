using System;

namespace CVBImageProc.MVVM
{
  /// <summary>
  /// ViewModel for a type.
  /// </summary>
  public class TypeViewModel : ViewModelBase
  {
    #region Properties

    /// <summary>
    /// Name of the type.
    /// </summary>
    public string Name => Type.Name;

    /// <summary>
    /// The wrapped type.
    /// </summary>
    public Type Type { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="type">Type to wrap.</param>
    public TypeViewModel(Type type)
    {
      Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    #endregion Construction

    /// <summary>
    /// Instantiates an object of the wrapped type.
    /// </summary>
    /// <returns></returns>
    public object Instanciate()
    {
      return Activator.CreateInstance(Type);
    }
  }
}