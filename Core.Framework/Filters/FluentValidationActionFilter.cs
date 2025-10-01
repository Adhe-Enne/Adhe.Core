using Core.Framework.Contracts.Api;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Framework.Filters
{
    public class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FluentValidationActionFilter> _logger;

        public FluentValidationActionFilter(IServiceProvider serviceProvider, ILogger<FluentValidationActionFilter> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogDebug("FluentValidationActionFilter capturando validaciones {Action}", context.ActionDescriptor.DisplayName);

            var errors = new Dictionary<string, List<string>>();

            foreach (var arg in context.ActionArguments)
            {
                var value = arg.Value;
                if (value == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(value.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;
                if (validator == null) continue;

                var validationContext = new ValidationContext<object>(value);
                var result = await validator.ValidateAsync(validationContext);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        if (!errors.ContainsKey(error.PropertyName))
                            errors[error.PropertyName] = new List<string>();
                        errors[error.PropertyName].Add(error.ErrorMessage);
                    }
                }
            }

            if (errors.Any())
            {
                _logger.LogDebug("Validaciones Encontradas: {Errors}", string.Join(" | ", errors.SelectMany(e => e.Value)));

                var apiResult = new ApiResult<Dictionary<string, List<string>>>
                {
                    HasError = true,
                    Data = errors,
                    Message = "Se ha producido uno o varios errores de validación de campos.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                context.Result = new ObjectResult(apiResult)
                {
                    StatusCode = (int) apiResult.StatusCode
                };

                return;
            }

            await next();
        }
    }
}