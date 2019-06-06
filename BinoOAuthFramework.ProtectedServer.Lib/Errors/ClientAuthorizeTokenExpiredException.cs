using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class ClientAuthorizeTokenExpiredException : Exception
    {
        public ClientAuthorizeTokenExpiredException() : base()
        {

        }

        public ClientAuthorizeTokenExpiredException(string message) : base(message)
        {

        }

        public ClientAuthorizeTokenExpiredException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
