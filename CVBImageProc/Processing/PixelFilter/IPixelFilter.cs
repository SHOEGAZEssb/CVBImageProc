namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Interface for an object checking validity of pixels.
  /// </summary>
  interface IPixelFilter
  {
    string Name { get; }

    bool Check(byte pixel);
  }
}