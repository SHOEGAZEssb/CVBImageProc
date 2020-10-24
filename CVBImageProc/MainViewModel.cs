using CVBImageProc.ImageSource;
using CVBImageProc.MVVM;
using CVBImageProc.Processing;
using CVBImageProc.Properties;
using Microsoft.Win32;
using Stemmer.Cvb;
using Stemmer.Cvb.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CVBImageProc
{
  /// <summary>
  /// ViewModel for MainView.
  /// </summary>
  public class MainViewModel : ViewModelBase
  {
    #region Commands

    /// <summary>
    /// Command for opening an image.
    /// </summary>
    public ICommand OpenImageCommand { get; }

    /// <summary>
    /// Command for opening a raw file.
    /// </summary>
    public ICommand OpenRawFileCommand { get; }

    /// <summary>
    /// Command for saving an image.
    /// </summary>
    public ICommand SaveImageCommand { get; }

    /// <summary>
    /// Command for saving the output image
    /// as a raw file.
    /// </summary>
    public ICommand SaveRawCommand { get; }

    /// <summary>
    /// Command for processing the image.
    /// </summary>
    public ICommand ProcessCommand { get; }

    /// <summary>
    /// Command for using the output image as input
    /// image.
    /// </summary>
    public ICommand UseOutputImageAsInputImageCommand { get; }

    #endregion Commands

    #region Properties

    /// <summary>
    /// ViewModel for the image source.
    /// </summary>
    public ImageSourceViewModelBase ImageSourceVM
    {
      get => _imageSourceVM;
      private set
      {
        if (ImageSourceVM != value)
        {
          // clean up old vm
          if (ImageSourceVM is IDisposable d)
            d.Dispose();
          if (ImageSourceVM is ChangingImageSourceViewModelBase c)
            c.CurrentImageChanged -= ImageSource_CurrentImageChanged;

          _imageSourceVM = value;
          if (ImageSourceVM is ChangingImageSourceViewModelBase nc)
            nc.CurrentImageChanged += ImageSource_CurrentImageChanged;
          NotifyOfPropertyChange();
          ImageChanged();
        }
      }
    }
    private ImageSourceViewModelBase _imageSourceVM;

    /// <summary>
    /// The image to edit.
    /// </summary>
    public Image InputImage => ImageSourceVM?.CurrentImage;

    /// <summary>
    /// The edited image.
    /// </summary>
    public Image OutputImage
    {
      get => _outputImage;
      private set
      {
        if (OutputImage != value)
        {
          _outputImage = value;
          NotifyOfPropertyChange();

          if (AutoSave)
            AutoSaveImage();
        }
      }
    }
    private Image _outputImage;

    /// <summary>
    /// If true, automatically processes the
    /// <see cref="InputImage"/> when processing
    /// is requested by the <see cref="ProcessingVM"/>.
    /// </summary>
    public bool AutoProcess
    {
      get => _autoProcess;
      set
      {
        if (AutoProcess != value)
        {
          _autoProcess = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private bool _autoProcess;

    /// <summary>
    /// If true autosaves newly processed images
    /// to the configured <see cref="AutoSavePath"/>.
    /// </summary>
    public bool AutoSave
    {
      get => _autoSave;
      set
      {
        if (AutoSave != value)
        {
          if (value)
          {
            var sfd = new SaveFileDialog
            {
              Filter = SystemInfo.ImageFileSaveFormatFilter
            };

            if (sfd.ShowDialog() ?? false)
            {
              AutoSavePath = sfd.FileName;
            }
            else
              return;
          }

          _autoSave = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private bool _autoSave;

    /// <summary>
    /// Path to save autosaved images to.
    /// </summary>
    public string AutoSavePath
    {
      get => _autoSavePath;
      set
      {
        if (AutoSavePath != value)
        {
          _autoSavePath = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private string _autoSavePath;

    /// <summary>
    /// ViewModel for the processing.
    /// </summary>
    public ProcessingViewModel ProcessingVM { get; }

    /// <summary>
    /// ViewModel for the status bar.
    /// </summary>
    public StatusBarViewModel StatusBarVM { get; }

    #endregion Properties

    #region Member

    /// <summary>
    /// Task to do the processing.
    /// </summary>
    private Task<Image> _processingTask;

    /// <summary>
    /// WindowManager used to display dialogs.
    /// </summary>
    private readonly IWindowManager _windowManager;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MainViewModel()
    {
      OpenImageCommand = new DelegateCommand((o) => OpenImage());
      OpenRawFileCommand = new DelegateCommand((o) => OpenRawFile());
      SaveImageCommand = new DelegateCommand((o) => SaveImage());
      SaveRawCommand = new DelegateCommand((o) => SaveRaw());
      ProcessCommand = new DelegateCommand((o) => Process().Forget());
      UseOutputImageAsInputImageCommand = new DelegateCommand((o) => UseOutputImageAsInputImage());

      AutoProcess = true;
      ProcessingVM = new ProcessingViewModel();
      ProcessingVM.UpdateImageInfoRequested += ProcessingVM_UpdateImageInfoRequested;
      ProcessingVM.ProcessingRequested += ProcessingVM_ProcessingRequested;
      StatusBarVM = new StatusBarViewModel();

      _windowManager = new WindowManager();
    }

    #endregion Construction

    /// <summary>
    /// Opens a new <see cref="InputImage"/>.
    /// </summary>
    private void OpenImage()
    {
      var ofd = new OpenFileDialog
      {
        InitialDirectory = Settings.Default.LastLoadPath
      };

      if (ofd.ShowDialog() ?? false)
      {
        try
        {
          ImageSourceVM = MakeImageSourceVM(ofd.FileName);
          Settings.Default.LastLoadPath = Path.GetDirectoryName(ofd.FileName);
        }
        catch (Exception ex)
        {
          if(MessageBox.Show($"Error opening image: {ex.Message}\r\nDo you want to try to import the raw file?", "Error opening file", MessageBoxButton.YesNo) 
            == MessageBoxResult.Yes)
          {
            OpenRawFile(ofd.FileName);
          }
        }
      }
    }

    /// <summary>
    /// Creates a ViewModel for the <paramref name="file"/>
    /// specific image source.
    /// </summary>
    /// <param name="file">File to create image source ViewModel for.</param>
    /// <returns>Newly created image source ViewModel.</returns>
    private static ImageSourceViewModelBase MakeImageSourceVM(string file)
    {
      string ext = Path.GetExtension(file);
      if (SystemInfo.ImageFileLoadFormatFilter.Contains(ext))
        return new StaticImageSourceViewModel(new StaticImageSource(Image.FromFile(file)));
      else if (SystemInfo.DeviceFileLoadFormatFilter.Contains(ext))
        return new VideoImageSourceViewModel(new VideoImageSource(DeviceFactory.Open(file)));
      else
        throw new ArgumentException("Unsupported file format", nameof(file));
    }

    /// <summary>
    /// Opens the raw file importer to import
    /// a raw file.
    /// </summary>
    private void OpenRawFile()
    {
      var ofd = new OpenFileDialog()
      {
        InitialDirectory = Settings.Default.LastRAWLoadPath
      };

      if (ofd.ShowDialog() ?? false)
        OpenRawFile(ofd.FileName);
    }

    /// <summary>
    /// Opens the given <paramref name="file"/> as raw file.
    /// </summary>
    /// <param name="file">File to open as raw file.</param>
    private void OpenRawFile(string file)
    {
      var vm = new RawFileImportViewModel(file);
      if (_windowManager.ShowDialog(vm) ?? false)
      {
        ImageSourceVM = new StaticImageSourceViewModel(new StaticImageSource(vm.ImportedImage));
        Settings.Default.LastRAWLoadPath = file;
      }
    }

    /// <summary>
    /// Saves the <see cref="OutputImage"/>.
    /// </summary>
    private void SaveImage()
    {
      if (OutputImage == null)
        return;

      var sfd = new SaveFileDialog
      {
        Filter = SystemInfo.ImageFileSaveFormatFilter,
        InitialDirectory = Settings.Default.LastSavePath
      };

      if (sfd.ShowDialog() ?? false)
      {
        try
        {
          OutputImage.Save(sfd.FileName);
          Settings.Default.LastSavePath = Path.GetDirectoryName(sfd.FileName);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error saving image: {ex.Message}");
        }
      }
    }

    /// <summary>
    /// Saves the <see cref="OutputImage"/> as raw file.
    /// </summary>
    private void SaveRaw()
    {
      if (OutputImage == null)
        return;

      var sfd = new SaveFileDialog()
      {
        InitialDirectory = Settings.Default.LastRAWSavePath
      };

      if (sfd.ShowDialog() ?? false)
      {
        try
        {
          File.WriteAllBytes(sfd.FileName, OutputImage.GetPixels().ToArray());
          Settings.Default.LastRAWSavePath = sfd.FileName;
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error saving as raw: {ex.Message}");
        }
      }
    }

    /// <summary>
    /// Saves the current <see cref="OutputImage"/>
    /// to the configured <see cref="AutoSavePath"/>.
    /// </summary>
    private void AutoSaveImage()
    {
      string fileName = string.Empty;
      try
      {
        fileName = FindFreeFileName(AutoSavePath);
        OutputImage.Save(fileName);
      }
      catch (Exception ex)
      {
        AutoSave = false;
        MessageBox.Show($"Error autosaving image to {fileName}: {ex.Message}");
      }
    }

    /// <summary>
    /// Gets the first free filename based
    /// on the given <paramref name="baseName"/>.
    /// </summary>
    /// <param name="baseName">Base filename to append index to.</param>
    /// <returns>Free filename with index appended.</returns>
    private static string FindFreeFileName(string baseName)
    {
      int i = 0;
      string ext = Path.GetExtension(baseName);
      string newName = baseName.Replace(ext, $"{i++:000}{ext}");
      while (File.Exists(newName))
      {
        newName = baseName.Replace(ext, $"{i++:000}{ext}");
      }

      return newName;
    }

    /// <summary>
    /// Runs the <see cref="InputImage"/> through
    /// the processors.
    /// </summary>
    private async Task Process()
    {
      if ((_processingTask != null && !_processingTask.IsCompleted) || InputImage == null)
        return;

      try
      {
        StatusBarVM.StatusMessage = "Processing...";

        var start = DateTime.Now;
        _processingTask = ProcessingVM.ProcessAsync(InputImage);
        OutputImage = await _processingTask.ConfigureAwait(false);
        var end = DateTime.Now;

        StatusBarVM.StatusMessage = $"Processing took {(end - start).TotalMilliseconds} ms";
      }
      catch (Exception ex)
      {
        StatusBarVM.StatusMessage = $"Error processing image: {ex.Message}";
      }
    }

    /// <summary>
    /// Sets the <see cref="InputImage"/>
    /// to the <see cref="OutputImage"/>.
    /// </summary>
    private void UseOutputImageAsInputImage()
    {
      if (OutputImage == null)
        return;

      ImageSourceVM = new StaticImageSourceViewModel(new StaticImageSource(OutputImage));
    }

    private void ImageChanged()
    {
      NotifyOfPropertyChange(nameof(InputImage));
      bool oldAutoProcess = AutoProcess;
      _autoProcess = false;
      ProcessingVM.UpdateImageInfo(InputImage);
      _autoProcess = oldAutoProcess;
      Process().Forget();
    }

    private void ImageSource_CurrentImageChanged(object sender, EventArgs e)
    {
      ImageChanged();
    }

    /// <summary>
    /// Updates the image info of the <see cref="ProcessingVM"/>
    /// when it requests it.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void ProcessingVM_UpdateImageInfoRequested(object sender, EventArgs e)
    {
      if (InputImage != null)
        ProcessingVM.UpdateImageInfo(InputImage);
    }

    /// <summary>
    /// Processes the <see cref="InputImage"/>
    /// when the <see cref="ProcessingVM"/> requests it
    /// and <see cref="AutoProcess"/> is enabled.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private async void ProcessingVM_ProcessingRequested(object sender, EventArgs e)
    {
      if (AutoProcess)
        await Process().ConfigureAwait(false);
    }
  }
}