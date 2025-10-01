namespace Core.Framework.Contracts.Api.Interfaces
{
    public interface ILoginResult : IApiResult
    {
        string Token { get; set; }
    }

    public interface ILoginResult<T> : ILoginResult
    {
        T UserData { get; set; }
    }
}
