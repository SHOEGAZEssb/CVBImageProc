using Stemmer.Cvb;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace CVBImageProcLib.Processing
{
  /// <summary>
  /// Processor for clearing planes.
  /// </summary>
  [DataContract]
  [DisplayName("Plane Clear")]
  public sealed class PlaneClear : IProcessor
  {
    #region IProcessor Implementation

    /// <summary>
    /// Name of the processor.
    /// </summary>
    public string Name => "Plane Clear";

    /// <summary>
    /// Clears the configured planes of the image.
    /// </summary>
    /// <param name="inputImage">Image to clear planes from.</param>
    /// <returns>Processed image.</returns>
    public Image Process(Image inputImage)
    {
      if (inputImage == null)
        throw new ArgumentNullException(nameof(inputImage));

      if (Clears == null)
        Clears = inputImage.Planes.Select(p => false).ToArray();

      var newImage = new Image(inputImage.Size, inputImage.Planes.Count, inputImage.Planes[0].DataType);

      for (int i = 0; i < inputImage.Planes.Count; i++)
      {
        if (!Clears[i])
          inputImage.Planes[i].CopyTo(newImage.Planes[i]);
      }

      return newImage;
    }

    #endregion IProcessor Implementation

    #region Properties

    /// <summary>
    /// Clear state per plane.
    /// </summary>
    [DataMember]
    public bool[] Clears { get; set; }

    #endregion Properties
  }
}