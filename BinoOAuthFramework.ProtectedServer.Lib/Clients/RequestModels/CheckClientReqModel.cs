using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Clients.RequestModels
{
    
    public class CheckClientReqModel
    {
        /// <summary>
        /// Client 與 Protected Server 的MAC值
        /// The Share mac info with client and protected server
        /// the mac must be check after decrypt using the share key of client and protected server
        /// </summary>
        public string ClientProtectedMac { get; set; }

        /// <summary>
        /// 客戶端識別Id
        /// </summary>
        public ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 失效時間
        /// </summary>
        public long ExpiredTime { get; set; }
    }
}
