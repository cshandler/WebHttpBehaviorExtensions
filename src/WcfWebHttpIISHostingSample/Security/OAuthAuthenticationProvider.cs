using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using OAuth;
using System.ServiceModel.Channels;
using System.Collections.Specialized;
using System.Web;
using WebHttpBehaviorExtensions.Security;

namespace WcfWebHttpIISHostingSample
{
    public class OAuthAuthenticationProvider : IAuthenticationProvider
    {
        public bool Authenticate(OperationContext operationContext)
        {
            bool Authenticated = false;

            string normalizedUrl;
            string normalizedRequestParameters;

            // to get the httpmethod

            HttpRequestMessageProperty requestProperty = (HttpRequestMessageProperty)(operationContext.RequestContext.RequestMessage).Properties[HttpRequestMessageProperty.Name];

            string httpmethod = requestProperty.Method;

            // HttpContext.Current is null, so forget about it 
            // HttpContext context = HttpContext.Current; 

            NameValueCollection pa = HttpUtility.ParseQueryString(operationContext.IncomingMessageProperties.Via.Query);

            if (pa != null && pa["oauth_consumer_key"] != null)
            {
                // to get uri without oauth parameters
                string uri = operationContext.IncomingMessageProperties
                .Via.OriginalString.Replace
                   (operationContext.IncomingMessageProperties
                .Via.Query, "");

                string consumersecret = "secret";

                OAuthBase oauth = new OAuthBase();

                string hash = oauth.GenerateSignature(
                    new Uri(uri),
                    pa["oauth_consumer_key"],
                    consumersecret,
                    null, // totken
                    null, //token secret
                    httpmethod,
                    pa["oauth_timestamp"],
                    pa["oauth_nonce"],
                    out normalizedUrl,
                    out normalizedRequestParameters
                    );
                Authenticated = pa["oauth_signature"] == hash;
            }
            return Authenticated;
        }
    }
}