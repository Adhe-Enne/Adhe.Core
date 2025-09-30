using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Framework.Exceptions
{
    public class ValidateException : Exception
    {
        public ValidateException(string Message) : base(Message)
        { }
    }
}
