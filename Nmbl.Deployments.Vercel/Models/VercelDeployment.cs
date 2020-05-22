using System;
using System.Collections.Generic;
using Nmbl.Deployments.Vercel.Extensions;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelDeployment
    {
      public string Uid { get; set; }
      public string Name { get; set; }
      public string Url { get; set; }
      public DateTime Created { get; set; }
      public VercelDeploymentState State { get; set; }
      public string Type { get; set; }
      public VercelUser Creator { get; set; }
      public int InstanceCount { get; set; }
      public DateTime? AliasAssigned { get; set; }
      public DateTime CreatedAt { get; set; }
      public string Target { get; set; }
      public string AliasError { get; set; }
      public Dictionary<string, string> Meta { get; set; }

      public VercelDeployment(VercelDeploymentApiModel apiModel) {
          Uid = apiModel.Uid;
          Name = apiModel.Name;
          Url = apiModel.Url;
          Created = apiModel.Created.ToDateTimeUtc();

          if (Enum.TryParse<VercelDeploymentState>(apiModel.State, ignoreCase: true, out var state))
          {
              State = state;
          }
          else
          {
              State = VercelDeploymentState.Unknown;
          }

          Type = apiModel.Type;
          Creator = apiModel.Creator;
          InstanceCount = apiModel.InstanceCount;
          AliasAssigned = apiModel.AliasAssigned?.ToDateTimeUtc();
          CreatedAt = apiModel.CreatedAt.ToDateTimeUtc();
          Target = apiModel.Target;
          AliasError = apiModel.AliasError;
          Meta = apiModel.Meta;
      }
    }
}
