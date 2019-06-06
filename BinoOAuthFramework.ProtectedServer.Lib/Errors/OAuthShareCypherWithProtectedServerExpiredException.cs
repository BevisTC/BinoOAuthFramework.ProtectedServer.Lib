using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class OAuthShareCypherWithProtectedServerExpiredException : Exception
    {
        public OAuthShareCypherWithProtectedServerExpiredException() : base()
        {

        }

        public OAuthShareCypherWithProtectedServerExpiredException(string message) : base(message)
        {

        }

        public OAuthShareCypherWithProtectedServerExpiredException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
