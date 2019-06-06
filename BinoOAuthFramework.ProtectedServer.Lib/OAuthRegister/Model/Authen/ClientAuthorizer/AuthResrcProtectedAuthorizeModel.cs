using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Authen.ClientAuthorizer
{
    public class AuthResrcProtectedAuthorizeModel
    {
        /// <summary>
        /// 客戶端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 資源保護者Id
        /// </summary>
        public string PortectedId { get; set; }

        /// <summary>
        /// 需更新至Protected Detail 授權計數
        /// </summary>
        public int ProcessScoreCurrentTimes { get; set; }

        /// <summary>
        /// 需更新至Protected Detail 的Hash Value
        /// </summary>
        public string ProcessScoreHashValue { get; set; }

        /// <summary>
        /// 回傳至Client 已供下次驗證的Token
        /// </summary>
        public string ClientRespCypherText { get; set; }
    }
}
