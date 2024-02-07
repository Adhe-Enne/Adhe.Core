using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Framework
{
    public class BizException : Exception
    {
        public BizException(string Message) : base(Message)
        { }

        public BizException(string Message, Exception ex) : base(Message, ex)
        { }

        public BizException(Exception ex) : base(ex.Message, ex)
        { }
    }
}
