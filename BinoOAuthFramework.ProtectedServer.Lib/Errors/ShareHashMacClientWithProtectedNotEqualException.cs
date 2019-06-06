using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors
{
    public class ShareHashMacClientWithProtectedNotEqualException : Exception
    {
        /// <summary>
        /// 例外建構
        /// </summary>
        public ShareHashMacClientWithProtectedNotEqualException() : base()
        {

        }

        public ShareHashMacClientWithProtectedNotEqualException(string message) : base(message)
        {

        }

        public ShareHashMacClientWithProtectedNotEqualException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
