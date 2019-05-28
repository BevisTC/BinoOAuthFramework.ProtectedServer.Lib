using System;
using System.Collections.Generic;
using System.Text;

namespace BinoOAuthFramework.ProtectedServer.Lib.OAuthRegister.Model.Authz.ResrcProtector
{
    public class ApiAuthorizeModel
    {
        public string ClientId { get; set; }

        public string Token { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }


        // 計算屬性
        public string ReqUrl
        {
            get
            {
                return string.Format("api/{0}/{1}", ControllerName, ActionName);
            }
        }
    }
}
