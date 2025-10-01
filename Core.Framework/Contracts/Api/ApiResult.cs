using Core.Framework.Contracts.Api.Interfaces;
using Core.Framework.Contracts.Shared.Result;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Core.Framework.Contracts.Api
{
    public class ApiResult : GenericResult, IApiResult 
    {
        const string DEFAULT_MESSAGE = "Successful";
        public HttpStatusCode StatusCode { get; set; }

        public ApiResult()
        {
            Message = DEFAULT_MESSAGE;
            StatusCode = HttpStatusCode.OK;
            HasError = false;
        }

        public ApiResult(string message, HttpStatusCode statusCode, bool hasError = false)
        {
            Message = message;
            StatusCode = statusCode;
            HasError = hasError;
        }

        public ApiResult(Exception ex)
        {
            HasError = true;
            Message = ex.Message;
            StatusCode = HttpStatusCode.InternalServerError;
        }
        public ApiResult SetError(string Message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            Set(Message, true);
            StatusCode = statusCode;

            return this;
        }

        public void Set(IApiResult From)
        {
            StatusCode = From.StatusCode;
            base.Set(From);
        }

        public string StatusDescription => new HttpResponseMessage(StatusCode).ReasonPhrase ?? string.Empty;
    }

    public class ApiResult<T> : ApiResult, IApiResult<T>
    {
        [JsonPropertyOrder(6)]
        public T Data { get; set; }

        public ApiResult<T> SetErrorResult(string Message)
        {
            Set(Message, true);

            return this;
        }
    }
}
