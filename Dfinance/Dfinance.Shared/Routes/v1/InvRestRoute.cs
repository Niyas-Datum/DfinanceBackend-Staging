using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Routes.v1
{
    public class InvRestRoute
    {
        public const string Root = "api";

        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";
        public class Restaurant
        {
            public const string Main = $"{Base}/restaurent";
            public const string SaveRest = $"{Main}/save";
            public const string UpdateRest = $"{Main}/update";
            public const string FillRestbyid = $"{Main}/fillbyid";
            public const string FillRest = $"{Main}/fill";
            public const string DelRest = $"{Main}/delete";
            public const string GetProducts = $"{Main}/fillitems";
            public const string GetLoadData = $"{Main}/getdata";
            public const string FillTable = $"{Main}/fillTable";
            public const string GetKitchenCategory = $"{Main}/getKitCat";
            public const string PrintKOT = $"{Main}/printKot";
            public const string SaveOption = $"{Main}/saveOptn";
            public const string UpdateOption = $"{Main}/updateOptn";
            public const string GetEmployee = $"{Main}/getWaiter";
            public const string GetSection = $"{Main}/getSection";
            public const string GetCategories = $"{Main}/getCat";
            public const string GetItemsByTransId = $"{Main}/getItem";
        }
    }
}
