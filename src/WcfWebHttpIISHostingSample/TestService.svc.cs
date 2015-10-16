using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfWebHttpIISHostingSample
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class TestService : ITestService
    {
        public string GetValue(Guid id)
        {
            return string.Format("Hello your guid is {0}", id);
        }

        public string GetParentId(int Id)
        {
            return string.Format("Hello your parent id is {0}", Id);
        }

        public string GetCurrentTime(Guid guid, int id)
        {
            return string.Format("Current Guid {0} have id {1}", guid, id);
        }

        public string GetData(string data)
        {
            return string.Format("Received data: {0}", data);
        }
    }
}
