using CVBImageProc.MVVM;
using CVBImageProc.Processing;
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
    /// The image to edit.
    /// </summary>
    public Image InputImage
    {
      get => _inputImage;
      private set
      {
        if (InputImage != value)
        {
          _inputImage = value;
          NotifyOfPropertyChange();
          ProcessingVM.UpdateImageInfo(InputImage);
          Process().Forget();
        }
      }
    }
    private Image _inputImage;

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
        Filter = SystemInfo.ImageFileLoadFormatFilter,
        FilterIndex = 7
      };

      if (ofd.ShowDialog() ?? false)
      {
        try
        {
          InputImage = Image.FromFile(ofd.FileName);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error opening image: {ex.Message}");
        }
      }
    }

    /// <summary>
    /// Opens the raw file importer to import
    /// a raw file.
    /// </summary>
    private void OpenRawFile()
    {
      var ofd = new OpenFileDialog();
      if (ofd.ShowDialog() ?? false)
      {
        var vm = new RawFileImportViewModel(ofd.FileName);
        if (_windowManager.ShowDialog(vm) ?? false)
          InputImage = vm.ImportedImage;
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
        Filter = SystemInfo.ImageFileSaveFormatFilter
      };

      if (sfd.ShowDialog() ?? false)
      {
        try
        {
          OutputImage.Save(sfd.FileName);
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

      var sfd = new SaveFileDialog();
      if (sfd.ShowDialog() ?? false)
      {
        try
        {
          File.WriteAllBytes(sfd.FileName, OutputImage.GetPixels().ToArray());
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error saving as raw: {ex.Message}");
        }
      }
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

      InputImage = OutputImage;
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