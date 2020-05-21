using Nmbl.Vercel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.OcModules.Vercel.ViewModels
{
    public class VercelAdminIndexViewModel
    {
        public IEnumerable<VercelDeployment> Deployments { get; set; }
    }
}
