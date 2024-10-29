
namespace Core.Contracts
{
    public interface IGenericResult
    {
        bool HasError { get; set; }
        string Message { get; set; }
        string Value { get; set; }

        public void Set(string Message, bool Error = false);
        public void Set(IGenericResult From);
    }
 
}