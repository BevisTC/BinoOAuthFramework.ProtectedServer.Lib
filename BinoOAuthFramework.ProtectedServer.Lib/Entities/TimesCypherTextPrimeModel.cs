using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Entities
{
    /// <summary>
    /// Client Times Cypher text prime model for encrypt use
    /// </summary>
    internal class TimesCypherTextPrimeModel
    {
        /// <summary>
        /// 客戶端識別Id
        /// </summary>
        internal ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 客戶端識別Id'
        /// </summary>
        internal ClientTempIdentityModel ClientTempIdPrime { get; set; }

        /// <summary>
        /// 目前授權計數
        /// </summary>
        public int CurrentTimes { get; set; }
    }
}
