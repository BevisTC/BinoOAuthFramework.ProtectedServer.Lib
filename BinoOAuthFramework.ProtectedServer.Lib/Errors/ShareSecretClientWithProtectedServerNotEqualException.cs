using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class ShareSecretClientWithProtectedServerNotEqualException : Exception
    {
        public ShareSecretClientWithProtectedServerNotEqualException() : base()
        {

        }

        public ShareSecretClientWithProtectedServerNotEqualException(string message) : base(message)
        {

        }

        public ShareSecretClientWithProtectedServerNotEqualException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
