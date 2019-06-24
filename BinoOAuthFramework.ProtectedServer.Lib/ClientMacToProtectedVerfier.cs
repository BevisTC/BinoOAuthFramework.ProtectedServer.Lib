using Binodata.Crypto.Lib.UseCases;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Clients.RequestModels;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Entities;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Errors;
using Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.OAuthRegister.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib
{
    public class ClientMacToProtectedVerfier
    {
        private ProtectedServerMemberClient memberClientModel;

        public ClientMacToProtectedVerfier(ProtectedServerMemberClient accessClientModel)
        {
            this.memberClientModel = accessClientModel;
        }

        /// <summary>
        /// Client 呼叫 Protected Server進行驗證
        /// </summary>
        /// <param name="reqModel"></param>
        public void Verify(CheckClientReqModel reqModel)
        {
            //用 ProtectedServerMemberClient 組出 HashMac 
            ClientTempIdentityModel clientTempId = new ClientTempIdentityModel()
            {
                ClientId = this.memberClientModel.ClientId,
                HashValue = this.memberClientModel.HashValue,
            };

            SymCryptoModel clientProtectedCryptoModel = new SymCryptoModel()
            {
                Key =  this.memberClientModel.ShareKeyClientWithProtectedServer,
                IV = this.memberClientModel.ShareIVClientWithProtectedServer,
            };

            ClientProtectedMacModel clientProtectedMacModel = new ClientProtectedMacModel();
            clientProtectedMacModel.Salt = "2";
            clientProtectedMacModel.ClientTempId = clientTempId;
            clientProtectedMacModel.ProtectedId = this.memberClientModel.ProtectedId;
            clientProtectedMacModel.AuthZTimes = this.memberClientModel.AuthZTimes;
            clientProtectedMacModel.HashValue = clientTempId.HashValue;
            clientProtectedMacModel.ExpiredTime = reqModel.ExpiredTime;
            clientProtectedMacModel.ClientProtectedCryptoModel = clientProtectedCryptoModel;

            string shareMacClientWithResrJson = JsonConvert.SerializeObject(clientProtectedMacModel);
            //組出HashMac
            string shareHashMacClientWithResr = MD5Hasher.Hash(shareMacClientWithResrJson);

            //檢核是否一致
            if (shareHashMacClientWithResr != reqModel.ClientProtectedMac)
            {
                throw new ShareHashMacClientWithProtectedNotEqualException("Client request mac in model is invalid. " +
                    "More message: the share mac in client is not equal after protected server decrypted and compare " +
                    "the mac message which client request");
            }

        }


    }

    internal class ClientProtectedMacModel
    {

        /// <summary>
        /// 加鹽值
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// TIDC
        /// </summary>
        public ClientTempIdentityModel ClientTempId { get; set; }

        /// <summary>
        /// 資源保護者Id
        /// </summary>
        public string ProtectedId { get; set; }

        /// <summary>
        /// 雜湊值
        /// </summary>
        public string HashValue { get; set; }

        /// <summary>
        /// 失效時間
        /// </summary>
        public long ExpiredTime { get; set; }

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
