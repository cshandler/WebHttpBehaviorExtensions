using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfHostConsole.Contract;
using WebHttpBehaviorExtensions;

namespace WcfHostConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:5000/Service";
            ServiceHost host = new ServiceHost(typeof(TestService), new Uri(baseAddress));

            var webendpoint = host.AddServiceEndpoint(typeof(ITestService), new WebHttpBinding(), "");
            webendpoint.Behaviors.Add(new TypedUriTemplateBehavior());

            host.Open();
            Console.WriteLine("Host opened");

            WebClient c = new WebClient();

            #region Tests
            Console.WriteLine(c.DownloadString(baseAddress + "/" + Guid.NewGuid().ToString()));

            Console.WriteLine(c.DownloadString(baseAddress + "/Parent/" + 20));

            Console.WriteLine(c.DownloadString(baseAddress + "/" + Guid.NewGuid() + "/Current/" + 10));

            Console.WriteLine(c.DownloadString(baseAddress + "/Data/" + "Mein_name_ist_WCF"));
            #endregion

            Console.Read();
        }
    }
}
