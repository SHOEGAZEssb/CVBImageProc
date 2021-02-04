﻿using CVBImageProcLib.Processing;
using System;

namespace CVBImageProcLib.Processing.Automation
{
  public interface IPropertyAutomation
  {
    event EventHandler ValueUpdated;

    double UpdatesPerSecond { get; set; }

    string PropertyName { get; }

    IProcessor Parent { get; set; }

    void Update();
  }
}