using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class RequestUrlNotInListException : Exception
    {
        public RequestUrlNotInListException() : base()
        {

        }

        public RequestUrlNotInListException(string message) : base(message)
        {

        }

        public RequestUrlNotInListException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
