using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Routes
{
    public static class InvRoute
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = $"{Root}/{Version}";
        public class TransactionAdditionals
        {
            public const string Main = $"{Base}/trnadd";
            public const string Save=$"{Main}/save";
            public const string Update=$"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string GetByTransactionId = $"{Main}/getByTrnId";
        }
        
    }
}
