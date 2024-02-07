using RestSharp;
using System;
using System.Threading.Tasks;

namespace Core.Externals
{
    public interface IRestSharpAdapter
    {
        string BaseUrl { set; }

        string Source { get; set; }

        Action<TraceMessage> Logger { get; set; }

        Task<RestResponse<T>> ExecuteCall<T>(string endpoint, RestRequest request) where T : new();

        Task<RestResponse<T>> ExecuteCall<T>(RestRequest request) where T : new();
    }
}
