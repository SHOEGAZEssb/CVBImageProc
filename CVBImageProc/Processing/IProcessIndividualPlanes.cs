namespace CVBImageProc.Processing
{
  interface IProcessIndividualPlanes : IProcessor
  {
    int PlaneIndex { get; set; }
  }
}