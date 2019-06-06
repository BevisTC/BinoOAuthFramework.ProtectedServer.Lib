using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common
{
    /// <summary>
    /// 加解密資料物件
    /// </summary>
    public class SymCryptoModel
    {
        public SymCryptoModel()
        {
        }

        public SymCryptoModel(string key, string iv)
        {
            Key = key;
            IV = iv;
        }

        /// <summary>
        /// 金鑰
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 初始化向量
        /// </summary>
        public string IV { get; set; }
    }
}
