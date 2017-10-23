using D365CrmExtensions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITDreamers.XRM.ConsoleTooling
{
    public class SPIntegrationEnvConfig : EnvConfig
    {
        public SPIntegrationEnvConfig()
        {
        }

        public SPIntegrationEnvConfig(string variablesGroupKey) : base(variablesGroupKey)
        {
        }

        public AuthConfig SPAuthConfig => new AuthConfig(Tenant, SharePointServerUri, ClientId, SPUserCredentials);

        public string Tenant => _envVariables[$"{VariablesGroupKey}_Tenant"].ToString();

        public Guid ClientId => new Guid(_envVariables[$"{VariablesGroupKey}_ClientId"].ToString());

        public Uri SharePointServerUri => new Uri(_envVariables[$"{VariablesGroupKey}_SPServerUri"].ToString());

        public UserCredential SPUserCredentials => new UserCredential(_envVariables[$"{VariablesGroupKey}_SPUserName"].ToString(),
            _envVariables[$"{VariablesGroupKey}_SPUserPassword"].ToString());

    }
}
