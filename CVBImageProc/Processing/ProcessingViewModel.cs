using CVBImageProc.MVVM;
using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace CVBImageProc.Processing
{
  /// <summary>
  /// ViewModel for the image processing.
  /// </summary>
  class ProcessingViewModel : ViewModelBase
  {
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

    #endregion Commands

    #region Properties

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
        }
      }
    }
    private IProcessorViewModel _selectedProcessor;

    #endregion Properties

    #region Member

    /// <summary>
    /// The processor chain to run through.
    /// </summary>
    private readonly ProcessorChain _processorChain;

    #endregion Member

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ProcessingViewModel()
    {
      AddProcessorCommand = new DelegateCommand((o) => AddSelectedProcessorType());
      RemoveProcessorCommand = new DelegateCommand((o) => RemoveSelectedProcessor());
      MoveProcessorUpCommand = new DelegateCommand((o) => MoveSelectedProcessorUp());
      MoveProcessorDownCommand = new DelegateCommand((o) => MoveSelectedProcessorDown());

      _processorChain = new ProcessorChain();
      Processors = new ObservableCollection<IProcessorViewModel>();
      Processors.CollectionChanged += Processors_CollectionChanged;

      AvailableProcessors = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(IProcessor))).Select(i => new TypeViewModel(i)).ToArray();
      SelectedProcessorType = AvailableProcessors.FirstOrDefault();
    }

    #endregion Construction

    /// <summary>
    /// Runs the given <paramref name="inputImage"/>
    /// through the processing chain.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      return _processorChain.Process(inputImage);
    }

    /// <summary>
    /// Adds an instance of the <see cref="SelectedProcessorType"/>
    /// to the <see cref="Processors"/>.
    /// </summary>
    private void AddSelectedProcessorType()
    {
      if (SelectedProcessorType == null)
        return;

      // add to model
      _processorChain.Processors.Add((IProcessor)SelectedProcessorType.Instanciate());

      // add to vm
      Processors.Add(CreateProcessorViewModel(_processorChain.Processors.Last()));
      SelectedProcessor = Processors.LastOrDefault();
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
      Processors.RemoveAt(index);
    }

    private static IProcessorViewModel CreateProcessorViewModel(IProcessor processor)
    {
      if (processor == null)
        throw new ArgumentNullException(nameof(processor));

      switch(processor)
      {
        case Binarise b:
          return new BinariseViewModel(b);
        default:
          return new ProcessorViewModel(processor);
      }
    }

    private void MoveSelectedProcessorUp()
    {
      if (SelectedProcessor == null || Processors.Count == 1 || Processors.IndexOf(SelectedProcessor) <= 0)
        return;

      int index = Processors.IndexOf(SelectedProcessor);
      _processorChain.Processors.Reverse(index - 1, 1);

      var tmp = Processors[index];
      Processors[index] = Processors[index - 1];
      Processors[index - 1] = tmp;
      SelectedProcessor = tmp;
    }

    private void MoveSelectedProcessorDown()
    {
      if (SelectedProcessor == null || Processors.Count == 1 || Processors.IndexOf(SelectedProcessor) < 0 || Processors.IndexOf(SelectedProcessor) == Processors.Count - 1)
        return;

      int index = Processors.IndexOf(SelectedProcessor);
      _processorChain.Processors.Reverse(index, 1);
      var tmp = Processors[index];
      Processors[index] = Processors[index + 1];
      Processors[index + 1] = tmp;
      SelectedProcessor = tmp;
    }

    private void Processors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if(e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach(var settingsProc in e.NewItems.OfType<IHasSettings>())
          settingsProc.SettingsChanged += SettingsProc_SettingsChanged;
      }
      else if(e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (var settingsProc in e.OldItems.OfType<IHasSettings>())
          settingsProc.SettingsChanged -= SettingsProc_SettingsChanged;
      }

      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void SettingsProc_SettingsChanged(object sender, EventArgs e)
    {
      ProcessingRequested?.Invoke(this, EventArgs.Empty);
    }
  }
}