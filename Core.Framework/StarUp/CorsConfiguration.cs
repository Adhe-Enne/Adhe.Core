using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Framework.StartUp
{
    public static class CorsConfiguration
    {
        public static void AddCorsConfiguration(this IServiceCollection services, string allowedOrigins, string[] withOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(allowedOrigins,
                    policy => policy
                        .WithOrigins(withOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }
    }
}
