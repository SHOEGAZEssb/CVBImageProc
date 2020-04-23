using Stemmer.Cvb;
using System;

namespace CVBImageProc.Processing.PixelFilter
{
  /// <summary>
  /// Boundaries when processing images.
  /// </summary>
  public struct ProcessingBounds : IEquatable<ProcessingBounds>
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

    #region Overrides

    /// <summary>
    /// Compares if the given <paramref name="obj"/>
    /// is equal to this instance.
    /// </summary>
    /// <param name="obj">Object to compare for equality.</param>
    /// <returns>True if the objects are equal, otherwise false.</returns>
    public override bool Equals(object obj)
    {
      return obj is ProcessingBounds b ? Equals(b) : false;
    }

    /// <summary>
    /// Compares if the given <paramref name="obj"/>
    /// is equal to this instance.
    /// </summary>
    /// <param name="obj">Object to compare for equality.</param>
    /// <returns>True if the objects are equal, otherwise false.</returns>
    public bool Equals(ProcessingBounds obj)
    {
      return StartY == obj.StartY && StartX == obj.StartX &&
             Height == obj.Height && Width == obj.Width;
    }

    /// <summary>
    /// Generates a unique hashcode based on the properties
    /// of this object.
    /// </summary>
    /// <returns>Generated hashcode.</returns>
    public override int GetHashCode()
    {
      return StartY.GetHashCode() | StartX.GetHashCode() | Height.GetHashCode() | Width.GetHashCode();
    }

    #endregion Overrides

    #region Operators

    /// <summary>
    /// Compares the two object for equality.
    /// </summary>
    /// <param name="lhs">First object to compare.</param>
    /// <param name="rhs">Second object to compare.</param>
    /// <returns>True if the objects are equal, otherwise false.</returns>
    public static bool operator ==(ProcessingBounds lhs, ProcessingBounds rhs)
    {
      return lhs.Equals(rhs);
    }

    /// <summary>
    /// Compares the two object for inequality.
    /// </summary>
    /// <param name="lhs">First object to compare.</param>
    /// <param name="rhs">Second object to compare.</param>
    /// <returns>True if the objects are not equal, otherwise false.</returns>
    public static bool operator !=(ProcessingBounds lhs, ProcessingBounds rhs)
    {
      return !lhs.Equals(rhs);
    }

    #endregion Operators
  }
}