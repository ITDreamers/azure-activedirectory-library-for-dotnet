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
        protected IDictionary _envVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);

        public EnvConfig()
        {
            
        }

        public EnvConfig(string variablesGroupKey)
        {
            VariablesGroupKey = variablesGroupKey;
        }

        public string VariablesGroupKey { get; set; }
        public string D365OrgUrl => _envVariables[$"{VariablesGroupKey}_D365_URL"].ToString();
        public string D365OrgLgn => _envVariables[$"{VariablesGroupKey}_D365_LGN"].ToString();
        public string D365OrgPswd => _envVariables[$"{VariablesGroupKey}_D365_PSWD"].ToString();

        public string CrmConnectionString
        {
            get
            {
                return $"Url={D365OrgUrl};Username={D365OrgLgn};Password={D365OrgPswd};AuthType=Office365;";
            }
        }
    }
}
