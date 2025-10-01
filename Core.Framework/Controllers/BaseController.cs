using AutoMapper;
using Core.Contracts.Exceptions;
using Core.Framework.Contracts.Api;
using Core.Framework.Contracts.Api.Interfaces;
using Core.Framework.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.Framework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController(ILogger log, IMapper mapper) : ControllerBase
    {
        protected ILogger _log = log;
        protected IMapper _mapper = mapper;

        protected IApiResult HandleSuccess(string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            message = $"{message ?? "Operacion Realizada con exito."} - {statusCode}";
            _log.LogInformation(message);

            return new ApiResult(message, statusCode);
        }

        protected IApiResult HandleSuccess(HttpStatusCode statusCode, string template, params object[] args)
        {
            var message = FormatMessage(template, args);

            return HandleSuccess(message, HttpStatusCode.OK);
        }

        protected IApiResult HandleSuccess(string template, params object[] args)
        {
            var message = FormatMessage(template, args);

            return HandleSuccess(message, HttpStatusCode.OK);
        }

        protected ActionResult<IApiResult> ResponseApi(IApiResult apiResult)
        {
            return StatusCode((int) apiResult.StatusCode, apiResult);
        }

        protected ActionResult<IApiResult<T>> ResponseApi<T>(IApiResult<T> apiResult)
        {
            return StatusCode((int)apiResult.StatusCode, apiResult);
        }

        protected ActionResult<ILoginResult<T>> ResponseApi<T>(ILoginResult<T> apiResult)
        {
            return StatusCode((int) apiResult.StatusCode, apiResult);
        }

        public static string FormatMessage(string template, params object[] args)
        {
            return string.Format(template, args);
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

            var httpCode = ((IBusinessException)ex).ErrorCode switch
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

            var result = new ApiResult().SetError($"{ex.Message} ({((IBusinessException)ex).Reason})", httpCode);
            message = $"{result.Message ?? "Error"} : {result.StatusCode} . Exception: {ex.Message}";
            _log.LogError(message, ex);

            return result;
        }

        #region Wrappers
        protected async Task<ActionResult<IApiResult<T>>> Execute<T>(Func<Task<IApiResult<T>>> action)
        {
            var result = new ApiResult<T>();

            try
            {
                result = (ApiResult<T>) await action();
            }
            catch (Exception ex) when (ex is IBusinessException bex)
            {
                result.Set(HandleServiceException(ex));
            }
            catch (Exception ex)
            {
                result.Set(HandleException(ex, HttpStatusCode.InternalServerError, Domain.ERROR));
            }

            return ResponseApi(result);
        }

        protected async Task<ActionResult<IApiResult>> HandleRequestAsync(Func<Task<IApiResult>> action)
        {
            try
            {
                var result = await action();
                return ResponseApi(result);
            }
            catch (Exception ex) when (ex is IBusinessException bex)
            {
                var errorResult = HandleServiceException(ex);
                return ResponseApi(errorResult);
            }
            catch (Exception ex)
            {
                var errorResult = new ApiResult().SetError(ex.Message, HttpStatusCode.InternalServerError);
                return ResponseApi(errorResult);
            }
        }

        protected async Task<ActionResult<IApiResult<T>>> HandleRequestAsync<T>(Func<Task<IApiResult<T>>> action)
        {
            try
            {
                var result = await action();
                return ResponseApi(result);
            }
            catch (Exception ex) when (ex is IBusinessException bex)
            {
                var errorResult = HandleServiceException(ex);
                var typedErrorResult = new ApiResult<T>();
                typedErrorResult.Set(errorResult);

                return ResponseApi(typedErrorResult);
            }
            catch (Exception ex)
            {
                var errorResult = new ApiResult<T>().SetError(ex.Message, HttpStatusCode.InternalServerError);
                var typedErrorResult = new ApiResult<T>();
                typedErrorResult.Set(errorResult);

                return ResponseApi(typedErrorResult);
            }
        }
        #endregion
    }
}
