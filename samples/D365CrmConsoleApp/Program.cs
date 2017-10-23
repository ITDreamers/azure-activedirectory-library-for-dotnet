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

    class ProgramEnvExtender
    {
        public static void ExportEnvConfig()
        {
        }

        public static void ImportEnvConfig()
        {
        }

        public static T GetEnvConfig<T>(string[] args) where T : EnvConfig, new()
        {
            var variablesGroupKey = args[0];
            return new T { VariablesGroupKey = variablesGroupKey };
        }
    }

    class Program : ProgramEnvExtender
    {
        public static void Main(string[] args)
        {
            try
            {
                var envConfig = GetEnvConfig<SPIntegrationEnvConfig>(args);

                var authConfig = new AuthConfig(envConfig.Tenant, new Uri(envConfig.SharePointUrl), envConfig.ClientId,
                    envConfig.SharePointLgn, envConfig.SharePointPswd);
                var activity = new GetSPListInfoActivity();
                activity.RunBusinessLogic(new ConsoleLogger(), authConfig);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
    }
}
