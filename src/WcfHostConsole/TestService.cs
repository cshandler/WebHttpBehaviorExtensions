using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfHostConsole.Contract;

namespace WcfHostConsole
{
    class TestService : ITestService
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
