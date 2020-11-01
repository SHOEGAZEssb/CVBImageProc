﻿using Stemmer.Cvb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CVBImageProcLib.Processing
{
  [DataContract]
  public abstract class AOIPlaneProcessorBase : IAOIPlaneProcessor
  {
    #region IAOIPlaneProcessor Implementation

    /// <summary>
    /// The name of this processor.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Index of the plane to invert.
    /// </summary>
    [DataMember]
    public int PlaneIndex { get; set; }

    /// <summary>
    /// If true, all planes get processed.
    /// </summary>
    [DataMember]
    public bool ProcessAllPlanes { get; set; }

    /// <summary>
    /// If true, uses the <see cref="AOI"/>
    /// while processing.
    /// </summary>
    [DataMember]
    public bool UseAOI { get; set; }

    /// <summary>
    /// The AOI to process.
    /// </summary>
    [DataMember]
    public Rect AOI { get; set; }

    /// <summary>
    /// Processes the <paramref name="inputImage"/>.
    /// </summary>
    /// <param name="inputImage">Image to process.</param>
    /// <returns>Processed image.</returns>
    public abstract Image Process(Image inputImage);

    #endregion IAOIPlaneProcessor Implementation
  }
}
