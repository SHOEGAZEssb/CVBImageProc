using CVBImageProc.MVVM;
using Stemmer.Cvb;
using System;
using System.Windows.Input;

namespace CVBImageProc
{
  /// <summary>
  /// Color of the image to create.
  /// </summary>
  enum ColorMode
  {
    /// <summary>
    /// Create a mono image.
    /// </summary>
    Mono,

    /// <summary>
    /// Create a rgb image.
    /// </summary>
    RGB
  }

  /// <summary>
  /// ViewModel for the <see cref="RawFileImporter"/>.
  /// </summary>
  class RawFileImportViewModel : ViewModelBase, IWindowContext
  {
    #region Properties

    #region Commands

    /// <summary>
    /// Command for confirming the import.
    /// </summary>
    public ICommand OKCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand CancelCommand { get; }

    #endregion Commands

    /// <summary>
    /// The file to import.
    /// </summary>
    public string File { get; }

    /// <summary>
    /// The imported image.
    /// </summary>
    public Image ImportedImage
    {
      get => _importedImage;
      private set
      {
        if (ImportedImage != value)
        {
          _importedImage = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private Image _importedImage;

    /// <summary>
    /// The width of the image to create.
    /// </summary>
    public int Width
    {
      get => _width;
      set
      {
        if (Width != value)
        {
          _width = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private int _width;

    /// <summary>
    /// The height of the image to create.
    /// </summary>
    public int Height
    {
      get => _height;
      set
      {
        if (Height != value)
        {
          _height = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private int _height;

    /// <summary>
    /// The color of the image to create.
    /// </summary>
    public ColorMode ColorMode
    {
      get => _colorMode;
      set
      {
        if (ColorMode != value)
        {
          _colorMode = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private ColorMode _colorMode;

    /// <summary>
    /// How rgb pixels should be read from the raw bytes.
    /// </summary>
    public RGBMode RGBMode
    {
      get => _rgbMode;
      set
      {
        if (RGBMode != value)
        {
          _rgbMode = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private RGBMode _rgbMode;

    /// <summary>
    /// Pixel value used as a fill when no raw bytes are left.
    /// </summary>
    public byte FillValue
    {
      get => _fillValue;
      set
      {
        if (FillValue != value)
        {
          _fillValue = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private byte _fillValue;

    /// <summary>
    /// The dialog result.
    /// </summary>
    public bool? Result
    {
      get => _result;
      private set
      {
        if (Result != value)
        {
          _result = value;
          NotifyOfPropertyChange();
          Import();
        }
      }
    }
    private bool? _result;

    /// <summary>
    /// Event that is fired when the view should be closed.
    /// </summary>
    public event EventHandler CloseRequested;

    #endregion Properties

    #region Member

    /// <summary>
    /// The raw bytes of the file.
    /// </summary>
    private readonly byte[] _rawBytes;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="file">The file to import.</param>
    public RawFileImportViewModel(string file)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException(nameof(file));

      File = file;
      _rawBytes = System.IO.File.ReadAllBytes(File);

      OKCommand = new DelegateCommand((o) => OK());
      CancelCommand = new DelegateCommand((o) => Cancel());

      _width = 800;
      _height = 800;
      _fillValue = 0;
      _colorMode = ColorMode.Mono;
      _rgbMode = RGBMode.RGBRGB;
      Import();
    }

    #endregion Construction

    /// <summary>
    /// Confirms the import.
    /// </summary>
    public void OK()
    {
      Result = true;
      CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Cancels the import.
    /// </summary>
    public void Cancel()
    {
      Result = false;
      CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Imports the file with the configured settings.
    /// </summary>
    private void Import()
    {
      ImportedImage = ColorMode == ColorMode.Mono ? RawFileImporter.ImportAsMono(_rawBytes, new Size2D(Width, Height), FillValue) :
                                                    RawFileImporter.ImportAsRGB(_rawBytes, new Size2D(Width, Height), FillValue, RGBMode);
    }
  }
}