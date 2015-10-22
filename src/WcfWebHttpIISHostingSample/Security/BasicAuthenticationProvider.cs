using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;
using WebHttpBehaviorExtensions.Security;

namespace WcfWebHttpIISHostingSample
{
    public class BasicAuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(System.ServiceModel.OperationContext operationContext)
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

            AuthenticationHeader header;

            if (AuthenticationHeader.TryDecode(authHeader, out header))
            {
                /*
                  This would be the place to inject the OAuth authentication manager. 
                */

                if ((header.Username == "user1" && header.Password == "test"))
                {
                    //User is authrized and originating call will proceed
                    return true;
                }
                else
                {
                    //not authorized
                    return false;
                }
            }
            else
            {
                //No authorization header was provided, so challenge the client to provide before proceeding:
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\" WcfWebHttpIISHostingSample\"");
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
}