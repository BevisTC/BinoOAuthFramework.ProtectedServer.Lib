using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Authen.ResrcProtector
{
    /// <summary>
    /// Resrc Protector 確認 Auth Server 回傳值 資料模型
    /// </summary>
    public class CheckAuthReqModel
    {
        /// <summary>
        /// Auth Server 傳送給 Protected Server 的 Cypher Text  [CT(AS->RO)]
        /// </summary>
        public string AuthProtectedCypherText { get; set; }
    }
}
