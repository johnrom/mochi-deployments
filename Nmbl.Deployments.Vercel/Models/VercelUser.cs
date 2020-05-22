using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelUser
    {
      public string Uid { get; set; }
      public string Email { get; set; }
      public string Username { get; set; }
      public string GithubLogin { get; set; }
    }
}
