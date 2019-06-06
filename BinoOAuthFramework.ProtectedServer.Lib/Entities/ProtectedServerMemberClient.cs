using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Entities
{
    /// <summary>
    /// 受保護伺服器 
    /// </summary>
    public class ProtectedServerMemberClient
    {
        /// <summary>
        /// 受保護的伺服器
        /// </summary>
        public string ProtectedId { get; set; }

        /// <summary>
        /// 可以存取 受保護伺服器 之裝置或設備 識別碼
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 裝置或設備 的雜湊
        /// </summary>
        public string HashValue { get; set; }

        /// <summary>
        ///  裝置或設備 的總授權次數
        /// </summary>
        public int AuthZTimes { get; set; }

        /// <summary>
        /// 裝置或設備 當前剩餘的授權次數
        /// </summary>
        public int CurrentTimes { get; set; }

        /// <summary>
        /// 裝置或設備 與 受保護伺服器間的共享 Key
        /// </summary>
        public string ShareKeyClientWithProtectedServer { get; set; }

        /// <summary>
        /// 裝置或設備 與 受保護伺服器間的共享 IV
        /// </summary>
        public string ShareIVClientWithProtectedServer { get; set; }
    }
}
