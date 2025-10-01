using Core.Contracts.Exceptions;
using Core.Framework.Contracts.Api;
using Core.Framework.Contracts.Api.Interfaces;
using Core.Framework.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.Framework.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _log;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _log = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (ex is IBusinessException bex)
            {
                await HandleExceptionAsync(HandleServiceException(ex), context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(HandleException(ex, HttpStatusCode.InternalServerError), context);
            }
        }

        private static Task HandleExceptionAsync(IApiResult result, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) result.StatusCode;
            return context.Response.WriteAsJsonAsync(result);
        }

        protected IApiResult HandleException(Exception ex, HttpStatusCode statusCode, string message = null)
        {
            message = $"{message ?? "Error"} : {statusCode} . Exception: {ex.Message}";
            _log.LogError(message, ex);

            return new ApiResult().SetError(message, statusCode);
        }

        protected IApiResult HandleServiceException(Exception ex)
        {
            string message;

            if (ex is not IBusinessException)
            {
                message = $"{Domain.ERROR} : {HttpStatusCode.InternalServerError} . Exception: {ex.Message}";
                _log.LogError(message, ex);
                return new ApiResult().SetError(message, HttpStatusCode.InternalServerError);
            }

            IBusinessException bex = (IBusinessException) ex;

            var httpCode = ((IBusinessException) ex).ErrorCode switch
            {
                EnumBusinessErrorCode.None => HttpStatusCode.OK,
                EnumBusinessErrorCode.UserAlreadyExists => HttpStatusCode.Conflict,
                EnumBusinessErrorCode.UserNotFound => HttpStatusCode.Unauthorized,
                EnumBusinessErrorCode.InvalidPassword => HttpStatusCode.Unauthorized,
                EnumBusinessErrorCode.InvalidRole => HttpStatusCode.BadRequest,
                EnumBusinessErrorCode.Unauthorized => HttpStatusCode.Unauthorized,
                EnumBusinessErrorCode.BoardNotFound => HttpStatusCode.NotFound,
                EnumBusinessErrorCode.ColumnNotFound => HttpStatusCode.NotFound,
                EnumBusinessErrorCode.TaskNotFound => HttpStatusCode.NotFound,
                EnumBusinessErrorCode.ValidationError => HttpStatusCode.BadRequest,
                EnumBusinessErrorCode.Forbidden => HttpStatusCode.Forbidden,
                EnumBusinessErrorCode.Conflict => HttpStatusCode.Conflict,
                EnumBusinessErrorCode.InternalError => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.BadRequest
            };

            var result = new ApiResult().SetError($"{ex.Message} ({((IBusinessException) ex).Reason})", httpCode);
            message = $"{result.Message ?? "Error"} : {result.StatusCode} . Exception: {ex.Message}";
            _log.LogError(message, ex);

            return result;
        }
    }
}
