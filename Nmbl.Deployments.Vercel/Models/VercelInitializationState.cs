using System;
using System.Collections.Generic;

namespace Nmbl.Vercel.Models
{
    public class VercelInitializationState
    {
      public bool ConfiguredDeployHook { get; set; }
      public bool ConfiguredApiToken { get; set; }
    }
}
