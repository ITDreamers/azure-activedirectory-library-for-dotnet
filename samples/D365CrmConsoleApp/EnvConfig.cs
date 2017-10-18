using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITDreamers.XRM.ConsoleTooling
{
    public class EnvConfig
    {
        protected string _variablesGroupKey = "YOUR_ORG";
        protected IDictionary _envVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);

        public EnvConfig(string variablesGroupKey)
        {
            _variablesGroupKey = variablesGroupKey;
        }

        public string D365OrgUrl => _envVariables[$"{_variablesGroupKey}_D365_URL"].ToString();
        public string D365OrgLgn => _envVariables[$"{_variablesGroupKey}_D365_LGN"].ToString();
        public string D365OrgPswd => _envVariables[$"{_variablesGroupKey}_D365_PSWD"].ToString();

        public string CrmConnectionString
        {
            get
            {
                return $"Url={D365OrgUrl};Username={D365OrgLgn};Password={D365OrgPswd};AuthType=Office365;";
            }
        }
    }
}
