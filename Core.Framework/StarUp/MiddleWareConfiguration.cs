using Core.Framework.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Core.Framework.StarUp
{
    public static class MiddleWareConfiguration
    {
        public static void AddMiddleWareConfiguration(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
