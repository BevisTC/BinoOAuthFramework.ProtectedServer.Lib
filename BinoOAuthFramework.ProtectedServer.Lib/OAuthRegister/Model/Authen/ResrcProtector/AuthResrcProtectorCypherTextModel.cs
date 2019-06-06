using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Authen.ResrcProtector
{

    public class AuthResrcProtectorCypherTextModel
    {
        /// <summary>
        /// 客戶端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客戶端識別Id
        /// </summary>
        public ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 資源保護者Id
        /// </summary>
        public string PortectedId { get; set; }

        /// <summary>
        /// 失效時間
        /// </summary>
        public long ExpiredTime { get; set; }

        /// <summary>
        /// 多次Hash後的值
        /// </summary>
        public string MultiHashValue { get; set; }

        /// <summary>
        /// 授權次數
        /// </summary>
        public int AuthZTimes { get; set; }

        /// <summary>
        ///  Client 與 Resource Protector 的KEY,IV資料物件
        /// </summary>
        public SymCryptoModel ClientProtectedCryptoModel { get; set; }

    }
}
