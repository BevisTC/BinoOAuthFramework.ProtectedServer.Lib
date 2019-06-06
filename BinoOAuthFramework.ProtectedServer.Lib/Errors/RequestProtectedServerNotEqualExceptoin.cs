using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class RequestProtectedServerNotEqualExceptoin : Exception
    {
        /// <summary>
        /// 例外建構
        /// </summary>
        public RequestProtectedServerNotEqualExceptoin() : base()
        {

        }

        public RequestProtectedServerNotEqualExceptoin(string message) : base(message)
        {

        }

        public RequestProtectedServerNotEqualExceptoin(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
