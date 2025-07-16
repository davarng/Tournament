using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }

        public ApiException(int statusCode, string title, string message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
