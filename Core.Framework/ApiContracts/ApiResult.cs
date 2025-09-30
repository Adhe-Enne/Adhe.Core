using Core.Framework.Result;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace Core.Framework.ApiContracts
{
    public class ApiResult : GenericResult, IApiResult 
    {
        const string DEFAULT_MESSAGE = "Successful";
        //public bool HasError { get; set; } //main status
        //public string Message { get; set; } // message
        public HttpStatusCode StatusCode { get; set; }

        public ApiResult()// : base(DEFAULT_MESSAGE)
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
        /*
        public void Set(string Message, bool Error = false)
        {
            this.Message = Message;
            HasError = Error;
        }*/
        /*
        public void Set(string Message, HttpStatusCode statusCode)
        {
            this.Message = Message;
            StatusCode = statusCode;
        }*/

        //public void AppendMessage(string line)
        //{
        //    if (Message == DEFAULT_MESSAGE) Message = string.Empty;

        //    Message += line + Environment.NewLine;
        //}

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

        //public bool IsSuccess() => !HasError;

        //public bool IsFailure() => HasError;
    }

    public class ApiResult<T> : ApiResult, IApiResult<T>
    {
        [JsonPropertyOrder(6)]
        public T Data { get; set; } // Usar 'required' para evitar CS8618 y cumplir con la interfaz

        public ApiResult<T> SetErrorResult(string Message)
        {
            Set(Message, true);

            return this;
        }
    }
}
