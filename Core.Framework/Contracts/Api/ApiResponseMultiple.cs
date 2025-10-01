using Core.Framework.Contracts.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Framework.Contracts.Api
{
    public class ApiResponseMultiple 
    {
        public ApiResponseMultiple()
        {
            Messages = new List<GenericResult>();
        }

        public List<GenericResult> Messages { get; set; }

        public class Error
        {
            public int Code { get; set; }
            public string Field { get; set; }
            public string Message { get; set; }
        }
    }
}
