using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Externals
{
    public static class RestClientOld
    {
        public static T Get<T>(string ApiUrl)
        {
            var client = new RestSharp.RestClient(ApiUrl);
            var request = new RestRequest(string.Empty, RestSharp.Method.Get);
            //request.AddHeader("authorization", string.Concat("Basic ", Token));

            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                T apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
                return apiResponse;
            }
            else
                throw new Exception("El http response no es válido: " + response.StatusDescription + ". " + response.ErrorMessage);
        }

        public static T Post<T>(string ApiUrl, object Body)
        {
            var client = new RestSharp.RestClient(ApiUrl);
            var request = new RestRequest(string.Empty, RestSharp.Method.Post);
            //request.AddHeader("authorization", string.Concat("Basic ", Token));
            request.AddJsonBody(Body);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                T apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
                return apiResponse;
            }
            else
                throw new Exception("El http response no es válido: " + response.StatusDescription + ". " + response.ErrorMessage);
        }

        public static T Patch<T>(string ApiUrl, object Body)
        {
            var client = new RestSharp.RestClient(ApiUrl);
            var request = new RestRequest();

            request.AddJsonBody(Body);

            RestResponse<T> response = client.Execute<T>(request, Method.Patch);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("El http response no es válido: " + response.StatusDescription + ". " + response.ErrorMessage);

            return response.Data;
        }
    }
}
