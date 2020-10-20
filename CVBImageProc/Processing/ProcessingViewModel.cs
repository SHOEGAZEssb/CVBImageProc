using CVBImageProc.MVVM;
using CVBImageProc.Processing.Filter;
using CVBImageProcLib.Processing;
using CVBImageProcLib.Processing.Filter;
using Microsoft.Win32;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the image processing.
  /// </summary>
  public class ProcessingViewModel : ViewModelBase, INeedImageInfo
  {
    #region INeedImageInfo Implementation

    /// <summary>
    /// Updates the image information.
    /// </summary>
    /// <param name="img">Image to pull info from.</param>
    public void UpdateImageInfo(Image img)
    {
      foreach (var proc in Processors.OfType<INeedImageInfo>())
        proc.UpdateImageInfo(img);
    }

    #endregion INeedImageInfo Implementation

    #region Commands

    /// <summary>
    /// Command for adding a new processor.
    /// </summary>
    public ICommand AddProcessorCommand { get; }

    /// <summary>
    /// Command for removing a processor.
    /// </summary>
    public ICommand RemoveProcessorCommand { get; }

    /// <summary>
    /// Command for moving a processor up in the chain.
    /// </summary>
    public ICommand MoveProcessorUpCommand { get; }

    /// <summary>
    /// Command for moving a processor down in the chain.
    /// </summary>
    public ICommand MoveProcessorDownCommand { get; }

    /// <summary>
    /// Command for saving the processor chain to file.
    /// </summary>
    public ICommand SaveProcessorChainCommand { get; }

    /// <summary>
    /// Command for opening a processor chain from file.
    /// </summary>
    public ICommand LoadProcessorChainCommand { get; }

    /// <summary>
    /// Command for cloning the currently selected processor.
    /// </summary>
    public ICommand CloneProcessorCommand { get; }

    #endregion Commands

    #region Properties

    /// <summary>
    /// Event that is fired when image info
    /// should be updated.
    /// </summary>
    public event EventHandler UpdateImageInfoRequested;

    /// <summary>
    /// Event that is fired when processing should
    /// be executed.
    /// </summary>
    public event EventHandler ProcessingRequested;

    /// <summary>
    /// List of available processors.
    /// </summary>
    public IEnumerable<TypeViewModel> AvailableProcessors { get; }

    /// <summary>
    /// Currently selected processor type to add.
    /// </summary>
    public TypeViewModel SelectedProcessorType
    {
      get => _selectedProcessorType;
      set
      {
        if (SelectedProcessorType != value)
        {
          _selectedProcessorType = value;
          NotifyOfPropertyChange();
        }
      }
    }
    private TypeViewModel _selectedProcessorType;

    /// <summary>
    /// List of added processors.
    /// </summary>
    public ObservableCollection<IProcessorViewModel> Processors { get; }

    /// <summary>
    /// Currently selected processor.
    /// </summary>
    public IProcessorViewModel SelectedProcessor
    {
      get => _selectedProcessor;
      set
      {
        if (SelectedProcessor != value)
        {
          _selectedProcessor = value;
          NotifyOfPropertyChange();
          (MoveProcessorUpCommand as DelegateCommand).OnCanExecuteChanged();
          (MoveProcessorDownCommand as DelegateCommand).OnCanExecuteChanged();
        }
      }
    }
    private IProcessorViewModel _selectedProcessor;

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor chain to run through.
    /// </summary>
    private ProcessorChain _processorChain;

    /// <summary>
    /// Gets the processor types for serialization.
    /// </summary>
    private Type[] GetSerializerTypes => AvailableProcessors.Select(p => p.Type).Concat(PixelFilter.PixelFilterChainViewModel.AvailableFilter
                                                                                .Concat(FilterViewModel.AvailableFilterTypes)
                                                            .Select(p => p.Type)).ToArray();

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProcessingViewModel()
    {
      AddProcessorCommand = new DelegateCommand((o) => AddSelectedProcessorType());
      RemoveProcessorCommand = new DelegateCommand((o) => RemoveSelectedProcessor());
      MoveProcessorUpCommand = new DelegateCommand((o) => MoveSelectedProcessorUp(), (o) => SelectedProcessor != null && Processors.Count > 1
                                                                                            && Processors.IndexOf(SelectedProcessor) > 0);
      MoveProcessorDownCommand = new DelegateCommand((o) => MoveSelectedProcessorDown(), (o) => SelectedProcessor != null && Processors.Count > 1
                                                                                                && Processors.IndexOf(SelectedProcessor) >= 0
                                                                                                && Processors.IndexOf(SelectedProcessor) != Processors.Count - 1);
      SaveProcessorChainCommand = new DelegateCommand((o) => SaveProcessorChain());
      LoadProcessorChainCommand = new DelegateCommand((o) => LoadProcessorChain());
      CloneProcessorCommand = new DelegateCommand((o) => CloneSelectedProcessor());

      _processorChain = new ProcessorChain();
      Processors = new ObservableCollection<IProcessorViewModel>();
      Processors.CollectionChanged += Processors_CollectionChanged;

      AvailableProcessors = System.Reflection.Assembly.GetAssembly(typeof(IProcessor)).GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(IProcessor)) && !mytype.IsInterface && !mytype.IsAbstract &&
                        !mytype.GetCustomAttributes(true).Any(a => a.GetType() == typeof(SubProcessorAttribute)))
                 .Select(i => new TypeViewModel(i)).OrderBy(t => t.Name).ToArray();
      SelectedProcessorType = AvailableProcessors.FirstOrDefault();
    }

    #endregion Construction

    /// <summary>
    /// Runs the given <paramref name="inputImage"/>
    /// through the processing chain.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public async Task<Image> ProcessAsync(Image inputImage)
    {
      return await Task.Run(() =>
      {
        try
        {
          var img = _processorChain.Process(inputImage);
          foreach (var p in Processors)
            p.IsFaulted = false;
          return img;
        }
        catch (ProcessingException ex)
        {
          Processors.Where(p => p.Processor == ex.Processor).First().IsFaulted = true;
          foreach (var p in Processors.Where(p => p.Processor != ex.Processor))
            p.IsFaulted = false;
          throw;
        }
      }).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an instance of the <see cref="SelectedProcessorType"/>
    /// to the <see cref="Processors"/>.
    /// </summary>
    private void AddSelectedProcessorType()
    {
      if (SelectedProcessorType == null)
        return;

      AddProcessor((IProcessor)SelectedProcessorType.Instanciate());
    }

    /// <summary>
    /// Adds the given <paramref name="processor"/> to the chain.
    /// </summary>
    /// <param name="processor">Processor to add.</param>
    private void AddProcessor(IProcessor processor)
    {
      // add to model
      _processorChain.Processors.Add(new KeyValuePair<IProcessor, bool>(processor, true));

      // add to vm
      Processors.Add(CreateProcessorViewModel(_processorChain.Processors.Last()));
      SelectedProcessor = Processors.LastOrDefault();
    }

    /// <summary>
    /// Creates a <see cref="IProcessorViewModel"/> based on
    /// the given <paramref name="kvp"/>s type.
    /// </summary>
    /// <param name="kvp">Processor to create ViewModel for.</param>
    /// <returns>ViewModel for the given <paramref name="kvp"/>.</returns>
    private static IProcessorViewModel CreateProcessorViewModel(KeyValuePair<IProcessor, bool> kvp)
    {
      if (kvp.Key == null)
        throw new ArgumentNullException(nameof(kvp));

      switch (kvp.Key)
      {
        case Binarise b:
          return new BinariseViewModel(b, kvp.Value);
        case BitShift b:
          return new BitshiftViewModel(b, kvp.Value);
        case Crop c:
          return new CropViewModel(c, kvp.Value);
        case FilterProcessor f:
          return new FilterViewModel(f, kvp.Value);
        case CVBImageProcLib.Processing.Math m:
          return new MathViewModel(m, kvp.Value);
        case MonoToMultiplane m:
          return new MonoToMultiplaneViewModel(m, kvp.Value);
        case Invert i:
          return new InvertViewModel(i, kvp.Value);
        case PixelShift p:
          return new PixelShiftViewModel(p, kvp.Value);
        case PlaneClear p:
          return new PlaneClearViewModel(p, kvp.Value);
        case Replace r:
          return new ReplaceViewModel(r, kvp.Value);
        case RGBToMono r:
          return new RGBToMonoViewModel(r, kvp.Value);
        case RGBFactors r:
          return new RGBFactorsViewModel(r, kvp.Value);
        case Resize r:
          return new ResizeViewModel(r, kvp.Value);
        case Rotate r:
          return new RotateViewModel(r, kvp.Value);
        case Shuffle s:
          return new ShuffleViewModel(s, kvp.Value);
        case Smear s:
          return new SmearViewModel(s, kvp.Value);
        case Sort s:
          return new SortViewModel(s, kvp.Value);
        default:
          return new ProcessorViewModel(kvp.Key, kvp.Value);
      }
    }

    /// <summary>
    /// Removes the <see cref="SelectedProcessor"/>
    /// from the <see cref="Processors"/>.
    /// </summary>
    private void RemoveSelectedProcessor()
    {
      if (SelectedProcessor == null)
        return;

      // remove from model
      int index = Processors.IndexOf(SelectedProcessor);
      _processorChain.Processors.RemoveAt(index);

      // remove from VM
      Processors.Remove(SelectedProcessor);

      if (Processors.ElementAtOrDefault(index) != null)
        SelectedProcessor = Processors.ElementAt(index);
      else if (Processors.ElementAtOrDefault(index - 1) != null)
        SelectedProcessor = Processors.ElementAt(index - 1);
    }

    /// <summary>
    /// Moves the <see cref="SelectedProcessor"/> up in the chain.
    /// </summary>
    private void MoveSelectedProcessorUp()
    {
      if (!MoveProcessorUpCommand.CanExecute(null))
        return;

      int index = Processors.IndexOf(SelectedProcessor);
      _processorChain.Processors.Reverse(index - 1, 2);

      var tmp = Processors[index];
      Processors[index] = Processors[index - 1];
      Processors[index - 1] = tmp;
      SelectedProcessor = tmp;
      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Moves the <see cref="SelectedProcessor"/> down in the chain.
    /// </summary>
    private void MoveSelectedProcessorDown()
    {
      if (!MoveProcessorDownCommand.CanExecute(null))
        return;

      int index = Processors.IndexOf(SelectedProcessor);
      _processorChain.Processors.Reverse(index, 2);
      var tmp = Processors[index];
      Processors[index] = Processors[index + 1];
      Processors[index + 1] = tmp;
      SelectedProcessor = tmp;
      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }

    #region Serialization

    /// <summary>
    /// Saves a <see cref="ProcessorChain"/> to file.
    /// </summary>
    private void SaveProcessorChain()
    {
      try
      {
        var sfd = new SaveFileDialog
        {
          Filter = "XML Files (*.xml) |*.xml"
        };

        if (sfd.ShowDialog() ?? false)
        {
          var settings = new DataContractSerializerSettings()
          {
            KnownTypes = GetSerializerTypes
          };
          var serializer = new DataContractSerializer(typeof(ProcessorChain), settings);

          var xmlSettings = new XmlWriterSettings { Indent = true };
          using (var w = XmlWriter.Create(sfd.FileName, xmlSettings))
            serializer.WriteObject(w, _processorChain);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error saving processor chain: {ex.Message}");
      }
    }

    /// <summary>
    /// Loads a <see cref="ProcessorChain"/> from file.
    /// </summary>
    private void LoadProcessorChain()
    {
      try
      {
        var ofd = new OpenFileDialog()
        {
          Filter = "XML Files (*.xml) |*.xml"
        };

        if (ofd.ShowDialog() ?? false)
        {
          var settings = new DataContractSerializerSettings()
          {
            KnownTypes = GetSerializerTypes
          };
          var serializer = new DataContractSerializer(typeof(ProcessorChain), settings);

          using (var fs = new FileStream(ofd.FileName, FileMode.Open))
            _processorChain = (ProcessorChain)serializer.ReadObject(fs);

          InitializeViewModels();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error loading processor chain: {ex.Message}");
      }
    }

    /// <summary>
    /// Clones the <see cref="SelectedProcessor"/>.
    /// </summary>
    private void CloneSelectedProcessor()
    {
      if (SelectedProcessor == null)
        return;

      try
      {
        var settings = new DataContractSerializerSettings()
        {
          KnownTypes = GetSerializerTypes
        };
        var serializer = new DataContractSerializer(typeof(ProcessorChain), settings);

        using (var ms = new MemoryStream())
        {
          // serialize to memory
          serializer.WriteObject(ms, SelectedProcessor.Processor);

          // clone from memory
          ms.Position = 0;
          AddProcessor((IProcessor)serializer.ReadObject(ms));
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error cloning processor: {ex.Message}");
      }
    }

    #endregion Serialization

    /// <summary>
    /// Initializes the <see cref="Processors"/>
    /// for the current <see cref="_processorChain"/>.
    /// </summary>
    private void InitializeViewModels()
    {
      // clear old processors
      foreach (var proc in Processors.ToArray())
        Processors.Remove(proc);

      // add new processors
      foreach (var proc in _processorChain.Processors)
        Processors.Add(CreateProcessorViewModel(proc));
    }

    /// <summary>
    /// Links / unlinks events when the processor collection changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Contains the event data.</param>
    private void Processors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (var proc in e.NewItems.OfType<IProcessorViewModel>())
        {
          proc.IsActiveChanged += Processor_IsActiveChanged;
          if (proc is IHasSettings settingsProc)
            settingsProc.SettingsChanged += SettingsProc_SettingsChanged;
        }

        if (e.NewItems.OfType<INeedImageInfo>().Any())
          UpdateImageInfoRequested?.Invoke(this, EventArgs.Empty);
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var proc in e.OldItems.OfType<IProcessorViewModel>())
        {
          proc.IsActiveChanged -= Processor_IsActiveChanged;
          if (proc is IHasSettings settingsProc)
            settingsProc.SettingsChanged -= SettingsProc_SettingsChanged;
        }
      }

      // don't fire when "replacing" (moveup, down)
      if (e.Action != NotifyCollectionChangedAction.Replace)
        ProcessingRequested?.Invoke(this, EventArgs.Empty);

      (MoveProcessorUpCommand as DelegateCommand).OnCanExecuteChanged();
      (MoveProcessorDownCommand as DelegateCommand).OnCanExecuteChanged();
    }

    /// <summary>
    /// Requests processing when a setting of a processor changed.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Ignored.</param>
    private void SettingsProc_SettingsChanged(object sender, EventArgs e)
    {
      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Processor_IsActiveChanged(object sender, EventArgs e)
    {
      var vm = sender as IProcessorViewModel;
      if (sender == null)
        return;

      int index = _processorChain.Processors.IndexOf(_processorChain.Processors.First(p => p.Key == vm.Processor));
      _processorChain.Processors[index] = new KeyValuePair<IProcessor, bool>(vm.Processor, vm.IsActive);
      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }
  }
}