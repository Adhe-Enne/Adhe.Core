using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Framework
{
    public class ApiResponseMultiple 
    {
        public ApiResponseMultiple()
        {
            this.Messages = new List<GenericResult>();
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
