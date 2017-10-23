using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365CrmExtensions
{
    public interface IAuthConfigSource
    {
        AuthConfig GetAuthConfig();
    }

    public class WfInputsAuthConfigSource : IAuthConfigSource
    {
        public WfInputsAuthConfigSource(GetSPListInfoActivity codeActivity, CodeActivityContext context)
        {
            Tenant = codeActivity.Tenant.Get<string>(context);
            ClientId = codeActivity.ClientId.Get<Guid>(context);
            SharePointServerUri = new Uri(codeActivity.SharePointServerUri.Get<string>(context));

            var userName = codeActivity.UserName.Get<string>(context);
            var userPassword = codeActivity.UserPassword.Get<string>(context);
            Credentials = new UserCredential(userName, userPassword);
        }

        public AuthConfig GetAuthConfig()
        {
            return new AuthConfig(Tenant, SharePointServerUri, ClientId, Credentials);
        }

        public Guid ClientId { get; }

        public Uri SharePointServerUri { get; }

        public UserCredential Credentials { get; }

        public string Tenant { get; }

    }

    public class AuthConfig
    {
        string _tenantId = string.Empty;

        public AuthConfig(string tenantId, Uri sharePointServerUrl, Guid clientId, UserCredential credentials)
        {
            _tenantId = tenantId;

            ClientId = clientId;
            Credentials = credentials;
            SharePointServerUri = sharePointServerUrl;
        }

        public AuthConfig(string tenantId, Uri sharePointServerUrl, Guid clientId, string userName, string userPassword)
        {
            _tenantId = tenantId;

            ClientId = clientId;
            Credentials = new UserCredential(userName, userPassword);
            SharePointServerUri = sharePointServerUrl;
        }

        /// <summary>
        /// The value in the path of the request can be used to control who can sign into the application.
        /// The allowed values are tenant identifiers, for example, 8eaef023-2b34-4da1-9baa-8bc8c9d6a490 or
        /// contoso.onmicrosoft.com or common for tenant-independent tokens
        /// </summary>
        public string TenantId => _tenantId == string.Empty ? "common" : _tenantId;
        public string Authority => $"https://login.microsoftonline.com/{TenantId}";

        public Guid ClientId { get; }

        public Uri SharePointServerUri { get; }

        public UserCredential Credentials { get; }

    }
}
