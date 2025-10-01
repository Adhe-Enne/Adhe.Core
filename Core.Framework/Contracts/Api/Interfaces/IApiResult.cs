using Core.Framework.Contracts.Shared.Result;
using System.Net;

namespace Core.Framework.Contracts.Api.Interfaces
{
    public interface IApiResult: IGenericResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; }
        public void Set(IApiResult From);
        public void AppendMessage(string line);
    }

    public interface IApiResult<T> : IApiResult
    {
        public T Data { get; set; }
    }
}
