using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WcfHostConsole.Contract
{
    [ServiceContract]
    public interface ITestService
    {
        [WebInvoke(Method = "GET", UriTemplate = "/Data/{data}")]
        string GetData(string data);

        [WebInvoke(Method = "GET", UriTemplate = "/{id}")]
        string GetValue(Guid id);

        [WebInvoke(Method = "GET", UriTemplate = "/Parent/{id}")]
        string GetParentId(int id);


        [WebInvoke(Method = "GET", UriTemplate = "/{guid}/Current/{id}")]
        string GetCurrentTime(Guid guid, int id);
    }
}
