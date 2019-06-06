using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class AuthorizeTimesHasRunOutException : Exception
    {
        public AuthorizeTimesHasRunOutException() : base()
        {

        }

        public AuthorizeTimesHasRunOutException(string message) : base(message)
        {

        }

        public AuthorizeTimesHasRunOutException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
