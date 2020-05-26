using Nmbl.Deployments.Vercel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.OrchardCore.Vercel.ViewModels
{
    public class VercelAdminIndexViewModel
    {
        public IEnumerable<VercelDeployment> Deployments { get; set; }
    }
}
