using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading;

namespace WebHttpBehaviorExtensions.Security
{
    /// <summary>
    /// Custom authorization manager for WCF Rest services which uses an ambient context to fetch the
    /// authentication logic via IAuthenticationProvider
    /// </summary>
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            return AuthContext.Current.Authenticate(operationContext);
        }
    }
}