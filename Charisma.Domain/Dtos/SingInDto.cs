using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Dtos
{
    public class SingInDto
    {
        public class Request
        {
            [NotNull]
            public string Username { get; set; }
            [NotNull]
            public string Password { get; set; }
        }

        public class Response
        {
            public string Token { get; set; }
        }
    }
}
