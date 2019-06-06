using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common
{
    public class ClientTempIdentityModel
    {

        public ClientTempIdentityModel()
        {

        }

        public ClientTempIdentityModel(string clientId, string hashValue)
        {
            this.ClientId = clientId;
            this.HashValue = hashValue;
        }

        /// <summary>
        /// 客戶端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// H^(n) * r   or  H^(n-i+1) * r 
        /// </summary>
        public string HashValue { get; set; }
    }
}
