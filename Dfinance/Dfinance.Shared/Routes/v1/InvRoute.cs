namespace Dfinance.Shared.Routes

{
    public static class InvRoute
    {
        public const string Root = "api";

        public const string Version = "v1";
        public const string Base = $"{Root}/{Version}";
        public static class WareHouse
        {
            public const string Main = $"{Base}/warehouse";
            public const string DropdownLocationTypes = $"{Main}/dropdown";
            public const string GetAll = $"{Main}/get";
            public const string GetById = $"{Main}/getbyid";
            public const string GetBWFill = $"{Main}/getBW";
            public const string Save = $"{Main}/Save";
            public const string Update = $"{Main}/Update";
            public const string Delete = $"{Main}/delete";
        }


        
        public class TransactionAdditionals
        {
            public const string Main = $"{Base}/trnadd";
            public const string Save=$"{Main}/save";
            public const string Update=$"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string GetByTransactionId = $"{Main}/getByTrnId";
        }
        public static class UnitMaster
        {
            public const string Main = $"{Base}/UnitMaster";

            public const string UnitPopup = $"{Main}/FillUnit";
            public const string FillMaster = $"{Main}/FillMaster";

            public const string FillByUnit = $"{Main}/FillByUnit";

            public const string UnitDropDown = $"{Main}/UnitDropDown";

            public const string SaveUnitMaster = $"{Main}/SaveUnitMaster";

            public const string UpdateUnitMaster = $"{Main}/UpdateUnitMaster";

            public const string DeleteUnitMaster = $"{Main}/DeleteUnitMaster";
        }

    }
}
