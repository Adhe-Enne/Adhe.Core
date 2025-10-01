using Core.Framework.Filters;
using Core.Framework.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Core.Framework.StarUp
{
    public static class ValidatorsConfiguration
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<FluentValidationActionFilter>();

            services.AddControllers(options =>
            {
                options.Filters.AddService<FluentValidationActionFilter>();
            });

            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleRequestValidator>();
        }
    }
}
