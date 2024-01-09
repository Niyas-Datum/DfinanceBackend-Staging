using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Routes.v1
{
    public class FinRoute
    {
        public const string Root = "api";

        public const string Version = "v1/fn";
        public const string Base = $"{Root}/{Version}";

        //CHART OF ACCOUNT ROUTE
        //short Name: COA
        public static class Coa
        {
            public const string Main = $"{Base}/coa";
            public const string SaveAccount = $"{Main}/saveacc";
            public const string UpdateAccount = $"{Main}/upacc";
            public const string Accountlist= $"{Main}/getaccs";
            public const string AccountsById = $"{Main}/getaccid";
            public const string SubGroup = $"{Main}/accsubgrps";
            public const string AccountGroup = $"{Main}/accgrps";
            public const string AccountCategory = $"{Main}/acccat";
            public const string DeleteAccount = $"{Main}/delacc";

        }

    }
}
