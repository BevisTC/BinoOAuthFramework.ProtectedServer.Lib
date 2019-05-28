using System;
using System.Collections.Generic;
using System.Text;

namespace BinoOAuthFramework.ProtectedServer.Lib.Entities
{
    /// <summary>
    /// 受保護伺服器 使用的領域模型
    /// </summary>
    public class ProtectedServerModel
    {
        /// <summary>
        /// Protected Server Applicaion in OAuth server is application id
        /// </summary>
        public string OAuthApplicatoinId { get; set; }

        /// <summary>
        /// Protected server name in OAuth is applicaion name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The private key shared with OAuth Server
        /// </summary>
        public string ShareKeyWithOAuthProtectedServer { get; set; }

        /// <summary>
        ///  The private IV shared with OAuth Server
        /// </summary>
        public string ShareIVWithOAuthProtectedServer { get; set; }
    }
}
