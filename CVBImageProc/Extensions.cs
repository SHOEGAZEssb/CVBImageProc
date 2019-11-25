using System.Threading.Tasks;

namespace CVBImageProc
{
  /// <summary>
  /// Extensions for the <see cref="Task"/> class.
  /// </summary>
  static class TaskExtensions
  {
#pragma warning disable IDE0060 // Remove unused parameter
                               /// <summary>
                               /// Explicitly states that we don't
                               /// want to do anything with the <paramref name="task"/>.
                               /// </summary>
                               /// <param name="task">task to forget.</param>
    public static void Forget(this Task task)
#pragma warning restore IDE0060 // Remove unused parameter
    { }
  }
}