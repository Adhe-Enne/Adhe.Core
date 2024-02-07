namespace Core.Framework
{
    public class ApiResult<T> : GenericResult
    {
        public T Data { get; set; }

        public ApiResult<T> SetErrorResult(string Message)
        {
            Set(Message, true);

            return this;
        }
    }
}
