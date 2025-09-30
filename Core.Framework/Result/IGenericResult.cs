namespace Core.Framework.Result
{
    public interface IGenericResult
    {
        bool HasError { get; set; }
        string Message { get; set; }
        string Value { get; set; }
        public void Set(string Message, bool Error = false);
        public void Set(IGenericResult From);
    }

    public interface IGenericResult<T> : IGenericResult
    {
        public T Data { get; set; }
    }
}