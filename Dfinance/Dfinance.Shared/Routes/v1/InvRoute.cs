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
            public const string DropdownBranch = $"{Main}/dropdownbranch";
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
		
		  public class InventroyTransactions
        {
            public const string Main = $"{Base}/invtrans";
            public const string getsalesman = $"{Main}/fillsalesman";
            public const string getvoucherno = $"{Main}/getvocno";
            public const string getreference = $"{Main}/getrefer";
            public const string DeletePurchase = $"{Main}/delete";
        }
        public class Purchase
        {
            public const string Main = $"{Base}/purchase";
            public const string Savepurchase = $"{Main}/savepurchase";
            public const string Updatepurchase = $"{Main}/updatepurchase";
            public const string Fillpurchasebyid = $"{Main}/fillpurchasebyid";
            public const string Fillpurchase = $"{Main}/fillpurchase";
            public const string Delpurchase = $"{Main}/delpurchase";
            public const string fillitems = $"{Main}/fillitems";
            public const string GetData = $"{Main}/getdata";
            public const string GetSupplier = $"{Main}/getsupplier";

        }
        public class InventoryPaymentTransaction
        {
            public const string Main = $"{Base}/invpaytrans";
            public const string FillTax = $"{Main}/popupTax";
            public const string FillAddCharge = $"{Main}/popupAddCharge";
            public const string FillCash = $"{Main}/popupCash";
            public const string FillCard = $"{Main}/popupCard";
            public const string FillEpay = $"{Main}/popupEpay";
            public const string FillAdvance = $"{Main}/popupAdvance";
            public const string FillCheque = $"{Main}/popupCheque";
            public const string FillBankName = $"{Main}/popupBankName";
            public const string SaveCheque = $"{Main}/SaveCheque";

        }
        public class InventoryItem
        {
            public const string Main = $"{Base}/invitem";
            public const string getItems = $"{Main}/getItems";
            public const string DeleteItems = $"{Main}/delItems";
            public const string ItemTransData = $"{Main}/itemtransdata";
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
