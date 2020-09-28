using System;

namespace CVBImageProc.ImageSource
{
  public interface IChangingImageSource : IImageSource
  {
    /// <summary>
    /// Event that is fired when the <see cref="IImageSource.CurrentImage"/> changed.
    /// </summary>
    event EventHandler CurrentImageChanged;
  }
}
