using Core.Contracts.Exceptions;
using Core.Framework.ApiContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Core.Framework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ILogger _log;

        public BaseController(ILogger log)
        {
            _log = log;
        }

        //protected IApiResult HandleException(ResultException ex, string message = null)
        //{
        //    message = $"{message ?? "Error"} : {ex.StatusCode} . Exception: {ex.Message}";
        //    _log.LogError(message, ex);

        //    return new ApiResult().SetError(message, ex.StatusCode);
        //}

        protected IApiResult HandleException(Exception ex, HttpStatusCode statusCode, string message = null)
        {
            message = $"{message ?? "Error"} : {statusCode} . Exception: {ex.Message}";
            _log.LogError(message, ex);

            return new ApiResult().SetError(message, statusCode);
        }

        protected IApiResult HandleSuccess(string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            message = $"{message ?? "Operacion Realizada con exito."} - {statusCode}";
            _log.LogInformation(message);

            return new ApiResult(message, statusCode);
        }

        protected IApiResult HandleSuccess(HttpStatusCode statusCode, string template, params object[] args)
        {
            var message = MessageFormatter.Format(template, args);

            return HandleSuccess(message, HttpStatusCode.OK);
        }

        protected IApiResult HandleSuccess(string template, params object[] args)
        {
            var message = MessageFormatter.Format(template, args);

            return HandleSuccess(message, HttpStatusCode.OK);
        }

        protected ObjectResult ResponseApi(IApiResult apiResult)
        {
            return StatusCode((int) apiResult.StatusCode, apiResult);
        }

        protected ObjectResult ResponseApi<T>(IApiResult<T> apiResult)
        {
            return StatusCode((int) apiResult.StatusCode, apiResult);
        }

        public static class MessageFormatter
        {
            public static string Format(string template, params object[] args)
            {
                return string.Format(template, args);
            }
        }

        protected IApiResult HandleServiceException(IBusinessException ex)
        {
            var httpCode = ex.ErrorCode switch
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
            return new ApiResult().SetError($"{ex.Message} ({ex.Reason})", httpCode);
        }
    }
}
