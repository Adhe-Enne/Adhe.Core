using RestSharp;
using System.Collections.Generic;

namespace Core.Externals
{
    public interface IFluentRestRequest
    {
        IFluentRestRequest UseResource(string resource);

        IFluentRestRequest AddBody(object payload);

        IFluentRestRequest UseEmptyBody();

        IFluentRestRequest AddHeaders(IDictionary<string, string> parameters);

        IFluentRestRequest AddParameters(IDictionary<string, string> parameters);

        RestRequest Done();
    }
}
