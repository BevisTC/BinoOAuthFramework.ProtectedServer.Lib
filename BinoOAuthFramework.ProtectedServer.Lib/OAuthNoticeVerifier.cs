using Binodata.Crypto.Lib;
using Binodata.Crypto.Lib.UseCases;
using Binodata.Crypto.Lib.Utility;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Entities;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Authen.ResrcProtector;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using Newtonsoft.Json;
using System;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib
{
    public class OAuthNoticeVerifier
    {
        private IAESCrypter aesCrypter;
        private ProtectedServerModel protectedServer;

        protected OAuthNoticeVerifier() { }

        public OAuthNoticeVerifier(ProtectedServerModel server)
        {
            this.aesCrypter = new LocalMachineAESCrypter(server.ShareKeyOAuthWithProtectedServer,
                   server.ShareIVOAuthWithProtectedServer);
            this.protectedServer = server;
        }

        public OAuthNoticeVerifier(IAESCrypter aescrypter, ProtectedServerModel server)
        {
            this.aesCrypter = aescrypter;
            this.protectedServer = server;
        }
        

        /// <summary>
        /// OAuth Server呼叫Protected Server進行通知驗證
        /// </summary>
        /// <param name="oauthToProtectedCypherText"></param>
        /// <returns></returns>
        public AuthResrcProtectorCypherTextModel Verify(string oauthToProtectedCypherText)
        {
            //解密OAuth Server帶過來的 CypherText
            string decryptCryptoStr = aesCrypter.Decrypt(oauthToProtectedCypherText);

            //進行反序列化，取得本次驗證需要的相關資訊
            AuthResrcProtectorCypherTextModel authShareProtectedServerCypherTextModel = JsonConvert.DeserializeObject<AuthResrcProtectorCypherTextModel>(decryptCryptoStr);

            //將Protected Server 的 Key與IV 進行加密處理，準備進行檢核
            SymCryptoModel shareSecretClientWithProtectedSymModel = GetShareSecretClientWithProtectedSymModel(this.protectedServer.ShareKeyOAuthWithProtectedServer,
                this.protectedServer.ShareIVOAuthWithProtectedServer, authShareProtectedServerCypherTextModel.ClientId);

            //檢核OAuth Server帶來的資料與 Protected Server處理後的資料 是否一致
            if (authShareProtectedServerCypherTextModel.PortectedId != this.protectedServer.OAuthApplicatoinId)
            {
                throw new RequestProtectedServerNotEqualExceptoin("The protected server's application id is not equal with the ProtectedId which is send from OAuth server ");
            }

            if (GetUtcNowUnixTime() > authShareProtectedServerCypherTextModel.ExpiredTime)
            {
                throw new OAuthShareCypherWithProtectedServerExpiredException("OAuth Send Secret message like Cypher text is expired, can not use this secret");
            }

            if(shareSecretClientWithProtectedSymModel.Key != authShareProtectedServerCypherTextModel.ClientProtectedCryptoModel.Key
                || shareSecretClientWithProtectedSymModel.IV != authShareProtectedServerCypherTextModel.ClientProtectedCryptoModel.IV)
            {
                throw new ShareSecretClientWithProtectedServerNotEqualException("Check the secret from OAuth Server, and found that the secret is not equal after decrypt by protected server");
            }
            
            return authShareProtectedServerCypherTextModel;
        }

        public virtual long GetUtcNowUnixTime()
        {
            return UnixTimeGenerator.GetUtcNowUnixTime();
        }

        private SymCryptoModel GetShareSecretClientWithProtectedSymModel(string key, string iv, string clientId)
        {
            string theShareSecretKeyClientWithProtected = GetClientProtectedCryptoStr(key, clientId);
            string theShareSecretIVClientWithProtected = GetClientProtectedCryptoStr(iv, clientId).Substring(0, 16);

            SymCryptoModel shareSecretClientWithProtectedModel = new SymCryptoModel(){
                Key = theShareSecretKeyClientWithProtected,
                IV = theShareSecretIVClientWithProtected,
            };

            return shareSecretClientWithProtectedModel;
        }

        private string GetClientProtectedCryptoStr(string cypherText, string clientId)
        {
            ShareInfoClientWithProtectedCryptoModel clientProtectedKeyModel = new ShareInfoClientWithProtectedCryptoModel
            {
                AuthProtectedCryptoStr = cypherText,
                ClientId = clientId
            };
            string clientProtectedKeyModelStr = JsonConvert.SerializeObject(clientProtectedKeyModel);
            string theShareSecretClientWithProtected = MD5Hasher.Hash(clientProtectedKeyModelStr);

            return theShareSecretClientWithProtected;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ShareInfoClientWithProtectedCryptoModel
    {
        /// <summary>
        /// key or iv
        /// </summary>
        public string AuthProtectedCryptoStr { get; set; }

        public string ClientId { get; set; }
    }
}
