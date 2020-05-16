using System;
using System.ComponentModel;
using System.Reflection;

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
    public string Name => _displayName ?? Type.Name;
    private readonly string _displayName;

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
      var dn = Type.GetCustomAttribute<DisplayNameAttribute>();
      if (dn != null)
        _displayName = dn.DisplayName;
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