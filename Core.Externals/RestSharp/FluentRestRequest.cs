using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace Core.Externals
{
    public class FluentRestRequest : IFluentRestRequest
    {
        private RestRequest _request;

        private FluentRestRequest(Method method)
        {
            _request = new RestRequest(string.Empty, method);
        }

        public static IFluentRestRequest CreatePost() { return new FluentRestRequest(Method.Post); }

        public static IFluentRestRequest CreateGet() { return new FluentRestRequest(Method.Get); }

        public static IFluentRestRequest CreatePut() { return new FluentRestRequest(Method.Put); }


        public IFluentRestRequest AddBody(object payload)
        {
            _request.AddJsonBody(payload);
            return this;
        }

        public IFluentRestRequest UseEmptyBody()
        {
            _request.AddJsonBody(new object());
            return this;
        }

        public IFluentRestRequest AddHeaders(IDictionary<string, string> parameters)
        {
            parameters.ToList().ForEach(param => _request.AddHeader(param.Key, param.Value));
            return this;
        }

        public IFluentRestRequest AddParameters(IDictionary<string, string> parameters)
        {
            parameters.ToList().ForEach(param => _request.AddParameter(param.Key, param.Value));
            return this;
        }

        public RestRequest Done()
        {
            return _request;
        }

        public IFluentRestRequest UseResource(string resource)
        {
            _request.Resource = resource;
            return this;
        }
    }
}
