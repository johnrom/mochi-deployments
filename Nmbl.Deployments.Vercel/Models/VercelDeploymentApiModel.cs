using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelDeploymentApiModel
    {
      public string Uid { get; set; }
      public string Name { get; set; }
      public string Url { get; set; }
      public long Created { get; set; }
      public string State { get; set; }
      public string Type { get; set; }
      public VercelUser Creator { get; set; }
      public int InstanceCount { get; set; }
      public long? AliasAssigned { get; set; }
      public long CreatedAt { get; set; }
      public string Target { get; set; }
      public string AliasError { get; set; }
      public Dictionary<string, string> Meta { get; set; }

      /*
      not using these yet
      "scale": {},
      */
    }
}
