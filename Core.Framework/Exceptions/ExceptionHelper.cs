using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Framework.Exceptions
{
    public static class ExceptionHelper
    {
        public static string GetMessage(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex.Message;
        }
    }
}
