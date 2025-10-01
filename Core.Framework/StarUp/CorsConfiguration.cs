using Microsoft.Extensions.DependencyInjection;

namespace Core.Framework.StarUp
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
