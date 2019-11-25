using CVBImageProc.Helper;
using CVBImageProc.MVVM;
using CVBImageProc.Processing;
using Microsoft.Win32;
using Stemmer.Cvb;
using Stemmer.Cvb.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CVBImageProc
{
  /// <summary>
  /// ViewModel for MainView.
  /// </summary>
  class MainViewModel : ViewModelBase, IHaveBusyStatus
  {
    #region IHaveBusyStatus Implementation

    /// <summary>
    /// Gets if the object is currently busy.
    /// </summary>
    public bool IsBusy
    {
      get => _isBusy;
      private set
      {
        if(IsBusy != value)
        {
          _isBusy = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private bool _isBusy;

    /// <summary>
    /// Gets the current busy message.
    /// </summary>
    public string BusyMessage
    {
      get => _busyMessage;
      private set
      {
        if(BusyMessage != value)
        {
          _busyMessage = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private string _busyMessage;

    #endregion IHaveBusyStatus Implementation

    #region Commands

    /// <summary>
    /// Command for opening an image.
    /// </summary>
    public ICommand OpenImageCommand { get; }

    /// <summary>
    /// Command for saving an image.
    /// </summary>
    public ICommand SaveImageCommand { get; }

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
        if(AutoProcess != value)
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

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MainViewModel()
    {
      OpenImageCommand = new DelegateCommand((o) => OpenImage());
      SaveImageCommand = new DelegateCommand((o) => SaveImage());
      ProcessCommand = new DelegateCommand((o) => Process().Forget());
      UseOutputImageAsInputImageCommand = new DelegateCommand((o) => UseOutputImageAsInputImage());

      AutoProcess = true;
      ProcessingVM = new ProcessingViewModel();
      ProcessingVM.UpdateImageInfoRequested += ProcessingVM_UpdateImageInfoRequested;
      ProcessingVM.ProcessingRequested += ProcessingVM_ProcessingRequested;
    }

    #endregion Construction

    /// <summary>
    /// Opens a new <see cref="InputImage"/>.
    /// </summary>
    private void OpenImage()
    {
      var ofd = new OpenFileDialog
      {
        Filter = SystemInfo.ImageFileLoadFormatFilter
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
    /// Runs the <see cref="InputImage"/> through
    /// the processors.
    /// </summary>
    private async Task Process()
    {
      try
      {
        IsBusy = true;
        BusyMessage = "Processing...";

        if (InputImage == null)
          return;

        try
        {
          OutputImage = await ProcessingVM.ProcessAsync(InputImage);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error processing image: {ex.Message}");
        }
      }
      finally
      {
        IsBusy = false;
        BusyMessage = null;
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
      if(AutoProcess)
        await Process();
    }
  }
}