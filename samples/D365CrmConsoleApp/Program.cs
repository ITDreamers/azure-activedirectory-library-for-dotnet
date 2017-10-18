using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using D365CrmExtensions;
using ITDreamers.XRM.ConsoleTooling;

namespace D365CrmConsoleApp
{
    public class ConsoleLogger : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var envConfig = new SPIntegrationEnvConfig("IT_DREAMERS");

                var activity = new GetSPListInfoActivity();
                activity.RunBusinessLogic(new ConsoleLogger(), envConfig.SPAuthConfig);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}
