using System;
using System.Collections.Generic;
using System.Text;

namespace Bino.ProtectedServer.OAuthClientCredentialsFlow.Lib.Clients.RequestModels
{
    public class RequestUrlModel
    {

        public RequestUrlModel(string actionName, string controllerName, RequestMode urlModel)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.UrlMode = urlModel;
        }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public RequestMode UrlMode { get; set; }

        public string GetRequestUrl()
        {
            if (UrlMode == RequestMode.Api)
            {
                return string.Format("api/{0}/{1}", ControllerName, ActionName);
            }
            else {
                return string.Format("{0}/{1}", ControllerName, ActionName);
            }
        }
    }


    public enum RequestMode
    {

        Api = 1,
        Web = 2

    }
}
