using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.ServiceModel;

namespace D365CrmConsoleApp
{
    class Program
    {
        private static CrmServiceClient _client;

        public static void Main(string[] args)
        {
            try
            {
                CrmServiceClient _client = new
                    CrmServiceClient(ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString);

                //Do stuff
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                string message = ex.Message;
                throw;
            }
        }
    }
}
