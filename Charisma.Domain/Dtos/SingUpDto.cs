using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Dtos
{
    public class SingUpDto
    {
        public class Request
        {
            [NotNull]
            public string FullName { get; set; }
            [NotNull]
            public string Email { get; set; }
            [NotNull]
            public string Password { get; set; }
        }

        public class Response
        {
            public string Token { get; set; }
        }
    }
}
