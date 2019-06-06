using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class TokenTicketCerticateException : Exception
    {
        public TokenTicketCerticateException() : base()
        {

        }

        public TokenTicketCerticateException(string message) : base(message)
        {

        }

        public TokenTicketCerticateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
