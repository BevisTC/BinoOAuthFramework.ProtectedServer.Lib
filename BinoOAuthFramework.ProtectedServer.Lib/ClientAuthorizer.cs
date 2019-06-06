using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Clients.RequestModels;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Entities;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Authen.ClientAuthorizer;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using Binodata.Crypto.Lib;
using Binodata.Crypto.Lib.UseCases;
using Binodata.Crypto.Lib.Utility;
using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib
{
    public class ClientAuthorizer
    {
        private ProtectedServerModel protectedServerEntity;
        private ProtectedServerMemberClient clientInProtectedMember;
        private RequestUrlModel requestUrlModel;
        private IAESCrypter aesCrypter;

        public ClientAuthorizer(ProtectedServerModel serverEntity,
            ProtectedServerMemberClient requestClientInMember, RequestUrlModel requestUrlModel, IAESCrypter aesCrypter)
        {
            this.protectedServerEntity = serverEntity;
            this.clientInProtectedMember = requestClientInMember;
            this.requestUrlModel = requestUrlModel;
            this.aesCrypter = aesCrypter;
        }

        public AuthResrcProtectedAuthorizeModel Verify(string token)
        {
            //解 Token
            string jwtDecodeValue = JWT.Decode(token,
                Encoding.Unicode.GetBytes(this.clientInProtectedMember.ShareKeyClientWithProtectedServer),
                JwsAlgorithm.HS256);
            ClientAuthorizedReqModel jwtObject = JsonConvert.DeserializeObject<ClientAuthorizedReqModel>(jwtDecodeValue);

            //加密後的合法 Url List
            List<string> encryptValueList = jwtObject.ValidUrlList;
            VerifyUrlIsInAuthorizedList(encryptValueList);


            ClientTempIdentityModel tempIdentityModel = new ClientTempIdentityModel(this.clientInProtectedMember.ClientId, this.clientInProtectedMember.HashValue);
            string shareKeyClientAndResrcDependsAuthorizedTimes = GetTempClientSecretByAuthorizedTimes(this.clientInProtectedMember.ShareKeyClientWithProtectedServer, tempIdentityModel, this.clientInProtectedMember.CurrentTimes);
            string shareIVClientAndResrcDependsAuthorizedTimes = GetTempClientSecretByAuthorizedTimes(this.clientInProtectedMember.ShareIVClientWithProtectedServer, tempIdentityModel, this.clientInProtectedMember.CurrentTimes);

            aesCrypter.SetKey(shareKeyClientAndResrcDependsAuthorizedTimes);
            aesCrypter.SetIV(shareIVClientAndResrcDependsAuthorizedTimes.Substring(0, 16));

            string clientAuthorizeCTCryptoDecrypt = aesCrypter.Decrypt(jwtObject.CurrentTimesCypherText);
            ClientCTCypherTextModelForAuthorize clientAuthorizeCypherTextModel = JsonConvert.DeserializeObject<ClientCTCypherTextModelForAuthorize>(clientAuthorizeCTCryptoDecrypt);
            

            if (GetUtcNowUnixTime() > clientAuthorizeCypherTextModel.ExpiredTime)
            {
                throw new ClientAuthorizeTokenExpiredException("Client authorized token has expired, please re-authenticate and get new token");
            }

            string protectedServerOriginalHash = this.clientInProtectedMember.HashValue;
            string doubleHashValue = MD5Hasher.Hash(clientAuthorizeCypherTextModel.HashValue);

            if (doubleHashValue != protectedServerOriginalHash)
            {
                throw new TokenTicketCerticateException("After checkt the token ticket, the token ticket is not right, the ticket you send has been used, please re-authenticate and get new token ticket");
            }

            //確認是否能夠取得下一次授權
            if (jwtObject.CurrentTimes + 1 >= clientInProtectedMember.AuthZTimes)
            {
                throw new AuthorizeTimesHasRunOutException("The token authorzie times has run out and expired, please re-authenticate and get new token ticket");
            }

            TimesCypherTextPrimeModel clientPrimeModel = new TimesCypherTextPrimeModel()
            {
                ClientTempIdPrime = new ClientTempIdentityModel() {
                    ClientId = clientInProtectedMember.ClientId,
                    HashValue = clientAuthorizeCypherTextModel.HashValue
                },
                CurrentTimes = clientInProtectedMember.CurrentTimes,
                ClientTempId = new ClientTempIdentityModel()
                {
                    ClientId = clientInProtectedMember.ClientId,
                    HashValue= clientInProtectedMember.HashValue,
                },
            };
            
            string newShareKeyClientAndProtected = GetTempClientSecretByAuthorizedTimes(clientInProtectedMember.ShareKeyClientWithProtectedServer, clientPrimeModel.ClientTempId, clientInProtectedMember.CurrentTimes);
            string newShareIVClientAndProtected = GetTempClientSecretByAuthorizedTimes(clientInProtectedMember.ShareIVClientWithProtectedServer, clientPrimeModel.ClientTempId, clientInProtectedMember.CurrentTimes).Substring(0,16);
            

            aesCrypter.SetIV(newShareIVClientAndProtected);
            aesCrypter.SetKey(newShareKeyClientAndProtected);
            string cypherPrimeStr = JsonConvert.SerializeObject(clientPrimeModel);
            string newCypherTextRespClientForNextAuthZ = aesCrypter.Encrypt(cypherPrimeStr);

            AuthResrcProtectedAuthorizeModel result = new AuthResrcProtectedAuthorizeModel()
            {
                ClientId = clientInProtectedMember.ClientId,
                PortectedId = clientInProtectedMember.ProtectedId,
                ProcessScoreCurrentTimes = (clientInProtectedMember.CurrentTimes + 1),
                ProcessScoreHashValue = clientAuthorizeCypherTextModel.HashValue,
                ClientRespCypherText = newCypherTextRespClientForNextAuthZ
            };
            return result;
        }

        public virtual long GetUtcNowUnixTime()
        {
            return UnixTimeGenerator.GetUtcNowUnixTime();
        }

        private string GetTempClientSecretByAuthorizedTimes(string shareScretClientWithProtectedServer,
            ClientTempIdentityModel tempIdentityModel,
            int currentTimes)
        {

            AuthorizeHashModel authorizeKeyHashModel = new AuthorizeHashModel
            {
                ClientProtectedCryptoStr = shareScretClientWithProtectedServer,
                ClientTempId = tempIdentityModel,
                CurrentTimes = currentTimes
            };


            string resrcClientKeyAuthZTimes = JsonConvert.SerializeObject(authorizeKeyHashModel);
            string hashValue = MD5Hasher.Hash(resrcClientKeyAuthZTimes);

            string authorizeCryptoStr = GetAuthorizeSecretModel(hashValue, resrcClientKeyAuthZTimes);
            string hashResult = MD5Hasher.Hash(authorizeCryptoStr);

            return hashResult;
        }

        private string GetAuthorizeSecretModel(string hashNMinusiAddOneMultiValue, string resrcClientKeyAuthZTimesStr)
        {
            AuthorizeKeyModel authorizeKeyModel = new AuthorizeKeyModel
            {
                HashKeyTIDCTimesValue = resrcClientKeyAuthZTimesStr,
                HashValue = hashNMinusiAddOneMultiValue
            };

            string crypto = JsonConvert.SerializeObject(authorizeKeyModel);

            return crypto;
        }

        private void VerifyUrlIsInAuthorizedList(List<string> encryptValueList)
        {
            //Set IAESCrypter Key and IV to prepare decrypt
            string requestUrl = this.requestUrlModel.GetRequestUrl();
            aesCrypter.SetKey(this.protectedServerEntity.ShareKeyOAuthWithProtectedServer);
            aesCrypter.SetIV(this.protectedServerEntity.ShareIVOAuthWithProtectedServer);

            //存入解密後的合法 Url List
            List<string> validRequestList = new List<string>();
            foreach (var cypherText in encryptValueList)
            {
                string decryptUrl = aesCrypter.Decrypt(cypherText);
                if ((decryptUrl == requestUrl) || (decryptUrl.Contains(requestUrl)))
                {
                    validRequestList.Add(decryptUrl);
                    break;
                }
            }

            //檢核是否為合法 Request
            if (validRequestList.Count <= 0)
            {
                throw new RequestUrlNotInListException("Client request the url is not in list or not found, " +
                    "please check the request url is in oauth list or the request url is exist");
            }
        }
    }


    internal class AuthorizeHashModel
    {

        /// <summary>
        ///  Auth Server 與 Resource Protector 的 Key 或 IV
        /// </summary>
        public string ClientProtectedCryptoStr { get; set; }

        /// <summary>
        /// 客戶端識別Id
        /// </summary>
        public ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 目前授權計數
        /// </summary>
        public int CurrentTimes { get; set; }
    }

    internal class AuthorizeKeyModel
    {

        /// <summary>
        /// Hash random value (n-i +1)
        /// </summary>
        public string HashValue { get; set; }

        /// <summary>
        /// Hash Resource Protector and Client Key , ClientTempId, Current Authorize Times
        /// </summary>
        public string HashKeyTIDCTimesValue { get; set; }
    }

    internal class ClientCTCypherTextModelForAuthorize
    {


        /// <summary>
        /// 客戶端識別Id
        /// </summary>
        public ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 資源保護者Id
        /// </summary>
        public string ProtectedId { get; set; }


        /// <summary>
        /// Hash random value (n-i)
        /// </summary>
        public string HashValue { get; set; }


        /// <summary>
        /// 失效時間
        /// </summary>
        public long ExpiredTime { get; set; }

    }
}
