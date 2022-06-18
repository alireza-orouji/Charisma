using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure.Core.Exceptions
{
    public class ApiResponseExeption : Exception
    {
        public int StatusCode { get; }

        public ApiResponseExeption(int StatusCode, string message) : base(message)
        {
            this.StatusCode = StatusCode;
        }
    }
}
