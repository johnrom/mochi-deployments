using Nmbl.Deployments.Vercel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Vercel.OcModule.ViewModels
{
    public class VercelAdminIndexViewModel
    {
        public IEnumerable<VercelDeployment> Deployments { get; set; }
    }
}
