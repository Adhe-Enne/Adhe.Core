using Core.Framework.Contracts.Api.Interfaces;
using System.Text.Json.Serialization;

namespace Core.Framework.Contracts.Api
{
    public class LoginResult : ApiResult, ILoginResult
    {
        [JsonPropertyOrder(7)]

        public string Token { get; set; }
    }

    public class LoginResult<T> : LoginResult, ILoginResult<T>
    {
        public LoginResult() : base()
        {
            this.Message = "Login successful";
        }
        [JsonPropertyOrder(6)]
        public T UserData { get; set; } = default!;
    }
}
