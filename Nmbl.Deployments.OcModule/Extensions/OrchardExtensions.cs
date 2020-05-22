using Microsoft.Extensions.DependencyInjection;
using OrchardCore;
using OrchardCore.Modules;
using System;
using System.Threading.Tasks;

namespace Nmbl.Vercel.OcModule.Extensions
{
    public static class OrchardExtensions
    {
        public static async Task<DateTime> ToLocalDateTimeAsync(this IOrchardHelper orchard, DateTime dateTime)
        {
            var clock = orchard.HttpContext.RequestServices.GetRequiredService<ILocalClock>();

            return (await clock.ConvertToLocalAsync(dateTime)).LocalDateTime;
        }
    }
}
