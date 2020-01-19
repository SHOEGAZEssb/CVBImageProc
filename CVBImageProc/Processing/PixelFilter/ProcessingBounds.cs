using Stemmer.Cvb;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Boundaries when processing images.
  /// </summary>
  public struct ProcessingBounds
  {
    #region Properties

    /// <summary>
    /// Row to start on.
    /// </summary>
    public int StartY { get; }

    /// <summary>
    /// Column to start on.
    /// </summary>
    public int StartX { get; }

    /// <summary>
    /// Amount of rows to process.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Amount of columns to process.
    /// </summary>
    public int Width { get; }

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="aoi">AOI to construct bounds from.</param>
    public ProcessingBounds(Rect aoi)
      : this(aoi.Location.Y, aoi.Location.X, aoi.Height, aoi.Width)
    { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="startY">Row to start on.</param>
    /// <param name="startX">Column to start on.</param>
    /// <param name="height">Amount of rows to process.</param>
    /// <param name="width">Amount of columns to process.</param>
    public ProcessingBounds(int startY, int startX, int height, int width)
    {
      StartY = startY;
      StartX = startX;
      Height = height;
      Width = width;
    }

    #endregion Construction
  }
}