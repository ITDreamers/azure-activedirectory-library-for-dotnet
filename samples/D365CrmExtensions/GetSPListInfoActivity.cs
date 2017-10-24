using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace D365CrmExtensions
{
    public class GetSPListInfoActivity : CodeActivity
    {
        #region Input Parameters

        [Input("Tenant")]
        [RequiredArgument]
        public InArgument<string> Tenant { get; set; }

        [Input("ClientId")]
        [RequiredArgument]
        public InArgument<string> ClientId { get; set; }

        [Input("SharePointServerUri")]
        [RequiredArgument]
        public InArgument<string> SharePointServerUri { get; set; }

        [Input("UserName")]
        [RequiredArgument]
        public InArgument<string> UserName { get; set; }

        [Input("UserPassword")]
        [RequiredArgument]
        public InArgument<string> UserPassword { get; set; }

        #endregion

        private static string GetAuthToken(AuthConfig authConfig)
        {
            var ac = new AuthenticationContext(authConfig.Authority);

            //string callbackUrl = "http://TestSP";
            //var secret = "";

            var resourceUri = authConfig.SharePointServerUri.ToString();
            var ar = ac.AcquireToken(resourceUri, authConfig.ClientId.ToString(), authConfig.Credentials);
            //AuthenticationResult ar = ac.AcquireToken(resourceUri, new ClientCredential(authConfig.ClientId.ToString(), secret));

            return ar.AccessToken;
        }

        public void RunBusinessLogic(ITracingService tracingService, AuthConfig authConfig)
        {
            tracingService.Trace("Start retriewing List info");

            var accesToken = GetAuthToken(authConfig);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var endpointUri = new Uri(authConfig.SharePointServerUri, string.Format("/_api/web/lists/getbytitle('{0}')", "Contacts"));
                var result = client.GetAsync(endpointUri).Result;
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var tracingService = context.GetExtension<ITracingService>();
            if (tracingService == null) { throw new InvalidPluginExecutionException("Failed to retrieve tracing service."); }

            tracingService.Trace("Entered code activity, Activity Instance Id: {0}, Workflow Instance Id: {1}",
                context.ActivityInstanceId, context.WorkflowInstanceId);

            var wfContext = context.GetExtension<IWorkflowContext>();
            if (wfContext == null) { throw new InvalidPluginExecutionException("Failed to retrieve workflow context."); }

            tracingService.Trace("Execute(), Correlation Id: {0}, Initiating User: {1}",
                wfContext.CorrelationId, wfContext.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            //IOrganizationService service = serviceFactory.CreateOrganizationService(wfContext.UserId);
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            try
            {
                var tenant = Tenant.Get<string>(context);
                var clientId = new Guid(ClientId.Get<string>(context));
                var sharePointServerUri = new Uri(SharePointServerUri.Get<string>(context));

                var userName = UserName.Get<string>(context);
                var userPassword = UserPassword.Get<string>(context);
                var credentials = new UserCredential(userName, userPassword);
                var authConfig = new AuthConfig(tenant, sharePointServerUri, clientId, credentials);

                RunBusinessLogic(tracingService, authConfig);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                tracingService.Trace("Exception: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("Exception: {0}", ex);
            }

            tracingService.Trace("Exiting GetRecordId.Execute(), Correlation Id: {0}", wfContext.CorrelationId);
        }
    }
}
