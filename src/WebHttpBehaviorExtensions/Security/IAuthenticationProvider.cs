using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.Collections.Specialized;
using System.Web;

namespace WebHttpBehaviorExtensions.Security
{
    /// <summary>
    /// Provides operations for custom Authentication provider.
    /// </summary>
    public interface IAuthenticationProvider
    {
        bool Authenticate(OperationContext operationContext);
    }
}
