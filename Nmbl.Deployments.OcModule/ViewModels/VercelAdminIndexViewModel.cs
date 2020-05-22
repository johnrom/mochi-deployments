using Nmbl.Vercel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.OcModule.ViewModels
{
    public class VercelAdminIndexViewModel
    {
        public IEnumerable<VercelDeployment> Deployments { get; set; }
    }
}
