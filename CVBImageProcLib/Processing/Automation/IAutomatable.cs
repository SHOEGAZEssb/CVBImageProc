using System.Collections.Generic;

namespace CVBImageProcLib.Processing.Automation
{
  /// <summary>
  /// Interface for an object whose
  /// properties can be automated.
  /// </summary>
  public interface IAutomatable
  {
    /// <summary>
    /// The configured automations.
    /// </summary>
    IReadOnlyList<IPropertyAutomation> Automations { get; }

    /// <summary>
    /// Adds the given <paramref name="automation"/>.
    /// </summary>
    /// <param name="automation">Automation to add.</param>
    void AddAutomation(IPropertyAutomation automation);

    /// <summary>
    /// Removes the given <paramref name="automation"/>.
    /// </summary>
    /// <param name="automation">Automation to remove.</param>
    void RemoveAutomation(IPropertyAutomation automation);
  }
}