using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WebHttpBehaviorExtensions;

namespace WcfWebHttpIISHostingSample
{
    [ServiceContract]
    public interface ITestService
    {
        [WebInvoke(Method = "GET", UriTemplate = "/Data/{data}")]
        string GetData(string data);

        [WebInvoke(Method = "GET", UriTemplate = "/{id}")]
        [UriTemplateSafe]
        string GetValue(Guid id);

        [WebInvoke(Method = "GET", UriTemplate = "/Parent/{id}")]
        [UriTemplateSafe]
        string GetParentId(int id);


        [WebInvoke(Method = "GET", UriTemplate = "/{guid}/Current/{id}")]
        [UriTemplateSafe]
        string GetCurrentTime(Guid guid, int id);
    }
}
