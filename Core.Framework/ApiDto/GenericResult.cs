using Core.Abstractions;
using System;

namespace Core.Framework
{
    public class GenericResult : Core.Abstractions.IGenericResult
    {
        const string DEFAULT_MESSAGE = "Successful";
        public bool HasError { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }

        public GenericResult(Exception ex)
        {
            this.HasError = true;
            this.Message = ex.Message;
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
            this.HasError = Error;
        }

        public GenericResult SetError(string Message)
        {
            Set(Message, true);

            return this;
        }

        public void Set(IGenericResult From)
        {
            this.HasError = From.HasError;
            this.Message = From.Message;
            this.Value = From.Value;
        }

        public bool IsSuccess() => !HasError;

        public bool IsFailure() => HasError;
    }
}
