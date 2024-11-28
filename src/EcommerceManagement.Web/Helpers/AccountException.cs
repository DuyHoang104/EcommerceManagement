using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceManagement.Web.Helpers
{
    public class AccountException : Exception
    {
        public AccountException() : base() { }

        public AccountException(string message) : base(message) { }

        public AccountException(string message, Exception innerException) : base(message, innerException) { }
    }
}