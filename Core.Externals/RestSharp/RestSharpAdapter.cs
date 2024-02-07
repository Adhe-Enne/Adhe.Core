using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Externals
{
    public sealed class RestSharpAdapter : IRestSharpAdapter, IDisposable
    {
        private RestClient _restClient;
        private Uri _baseUrl;

        public string BaseUrl
        {
            set
            {
                _baseUrl = new Uri(value);
            }
        }

        public string Source { get; set; } = string.Empty;

        public Action<TraceMessage> Logger { get; set; }

        public RestSharpAdapter()
        {
            _restClient = new RestClient();
            _restClient.UseSystemTextJson(new JsonSerializerOptions(JsonSerializerDefaults.Web)); // Use camel case
        }

        private async Task<RestResponse<T>> ExecuteCall<T>(Uri endpoint, RestRequest request) where T : new()
        {
            _restClient.Options.BaseUrl = endpoint;

            var traceId = Guid.NewGuid();
            var body = request.Parameters.Where(x => x.Type == ParameterType.RequestBody).SingleOrDefault();
            var logRequest = new TraceMessage
            {
                TraceId = traceId,
                Source = this.Source,
                Type = "REQUEST",
                URI = _restClient.Options.BaseUrl.AbsoluteUri + request.Resource,
                Method = request.Method.ToString(),
                ContentType = "application/json; charset=utf-8",
                ContentValue = Newtonsoft.Json.JsonConvert.SerializeObject(body?.Value)
            };

            TraceInfo(logRequest);

            RestResponse<T> response = await _restClient.ExecuteAsync<T>(request);

            TraceResponseInfo(logRequest, response);

            CheckJsonParsing<T>(response);

            return response;
        }

        private void CheckJsonParsing<T>(RestResponse<T> response)
        {
            if (response.ErrorException is null) return;

            if (response.ErrorException is System.Text.Json.JsonException)
                throw response.ErrorException;
        }

        private void TraceInfo(TraceMessage log)
        {
            if (Logger is null) return;

            Logger(log);
        }

        private void TraceResponseInfo(TraceMessage logRequest, RestResponse response)
        {
            var log = new TraceMessage
            {
                TraceId = logRequest.TraceId,
                Source = logRequest.Source,
                Type = "RESPONSE",
                URI = logRequest.URI,
                Method = logRequest.Method,
                ContentType = response.ContentType,
                ContentValue = $"Status={(int)response.StatusCode}; Content={response.Content}",
                Error = !response.IsSuccessful || response.ErrorException != null,
                Exception = GetExceptionDetails(response.ErrorException)
            };

            TraceInfo(log);
        }

        public async Task<RestResponse<T>> ExecuteCall<T>(string endpoint, RestRequest request) where T : new()
        {
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out Uri uri))
                throw new FormatException("Url not valid");

            return await ExecuteCall<T>(uri, request);
        }

        public async Task<RestResponse<T>> ExecuteCall<T>(RestRequest request) where T : new()
        {
            return await ExecuteCall<T>(_baseUrl, request);
        }

        public void Clear()
        {
            _restClient = new RestClient();
        }

        public void Dispose()
        {
            _restClient.Dispose();
        }

        private static string GetExceptionDetails(Exception ex)
        {
            if (ex is null) return null;

            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex.Message;
        }
    }
}
