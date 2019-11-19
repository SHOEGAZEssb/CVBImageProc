using CVBImageProc.MVVM;
using Microsoft.Win32;
using Stemmer.Cvb;
using Stemmer.Cvb.Utilities;
using System;
using System.Windows;
using System.Windows.Input;

namespace CVBImageProc
{
  /// <summary>
  /// ViewModel for MainView.
  /// </summary>
  class MainViewModel : ViewModelBase
  {
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

    #endregion Properties

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MainViewModel()
    {
      OpenImageCommand = new DelegateCommand((o) => OpenImage());
      SaveImageCommand = new DelegateCommand((o) => SaveImage());
      UseOutputImageAsInputImageCommand = new DelegateCommand((o) => UseOutputImageAsInputImage());
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

    private void UseOutputImageAsInputImage()
    {
      if (OutputImage == null)
        return;

      InputImage = OutputImage;
    }
  }
}