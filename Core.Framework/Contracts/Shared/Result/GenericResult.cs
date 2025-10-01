using System;

namespace Core.Framework.Contracts.Shared.Result
{
    public class GenericResult : IGenericResult
    {
        const string DEFAULT_MESSAGE = "Successful";
        public bool HasError { get; set; }
        public string Message { get; set; }

        public GenericResult(Exception ex)
        {
            HasError = true;
            Message = ex.Message;
        }

        public GenericResult()
        {
            Message = DEFAULT_MESSAGE;
        }

        public GenericResult(string Message, bool Error = false)
        {
            Set(Message, Error);
        }

        public void AppendMessage(string line)
        {
            if (Message == DEFAULT_MESSAGE) Message = string.Empty;

            Message += line + Environment.NewLine;
        }

        public void Set(string Message, bool Error = false)
        {
            this.Message = Message;
            HasError = Error;
        }

        public GenericResult SetError(string Message)
        {
            Set(Message, true);

            return this;
        }

        public void Set(IGenericResult From)
        {
            HasError = From.HasError;
            Message = From.Message;
        }

        public bool IsSuccess() => !HasError;

        public bool IsFailure() => HasError;
    }
}
