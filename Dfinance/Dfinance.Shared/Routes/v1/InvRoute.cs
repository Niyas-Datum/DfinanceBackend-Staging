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
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string GetByTransactionId = $"{Main}/getByTrnId";
            public const string transpType = $"{Main}/transpType";
            public const string salesArea = $"{Main}/salesArea";
            public const string vehicleNo = $"{Main}/vehicleNo";
            public const string delLoc = $"{Main}/delLoc";
        }

        public class InventroyTransactions
        {
            public const string Main = $"{Base}/invtrans";
            public const string getsalesman = $"{Main}/fillsalesman";
            public const string getvoucherno = $"{Main}/getvocno";
            public const string getreference = $"{Main}/getrefer";
            public const string Cancel = $"{Main}/cancel";
            public const string DeleteTrans = $"{Main}/delete";
            public const string FillVoucherType = $"{Main}/fillVoucherType";
            public const string FillRefItems = $"{Main}/fillRefItems";
            public const string refItemList = $"{Main}/refItemList";
            public const string FillRef = $"{Main}/fillRef";
            public const string payType = $"{Main}/payType";
            public const string GetInventoryTransactions = $"{Main}/getInvTrans";
            public const string partyBal = $"{Main}/partyBal";


        }
        public class Purchase
        {
            public const string Main = $"{Base}/purchase";
            public const string Savepurchase = $"{Main}/savepurchase";
            public const string Updatepurchase = $"{Main}/updatepurchase";
            public const string Fillpurchasebyid = $"{Main}/fillpurchasebyid";
            public const string Fillpurchase = $"{Main}/fillpurchase";
            public const string Cancelpurchase = $"{Main}/Cancelphs";
            public const string Delpurchase = $"{Main}/delpurchase";
            public const string fillitems = $"{Main}/fillitems";
            public const string GetData = $"{Main}/getdata";
            public const string GetSupplier = $"{Main}/getsupplier";
            public const string getPurchaseReport = $"{Main}/getPuReport";
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
            public const string defaultAcc = $"{Main}/defaultAcc";
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

        public class PurchaseOrder
        {
            public const string Main = $"{Base}/purchaseOrder";
            public const string SavePO = $"{Main}/savePO";
            public const string UpdatePO = $"{Main}/updatePO";
            public const string DeletePO = $"{Main}/deletePO";
            public const string CancelPO = $"{Main}/Cancel";
        }
        public class Sales
        {
            public const string Main = $"{Base}/sales";
            public const string SaveSales = $"{Main}/saveSales";
            public const string UpdateSales = $"{Main}/updateSales";
            public const string FillSalesbyid = $"{Main}/fillsalebyid";
            public const string FillSales = $"{Main}/fillsales";
            public const string DelSales = $"{Main}/delsales";
            public const string fillitems = $"{Main}/fillitems";
            public const string GetData = $"{Main}/getdata";
            public const string GetCustomer = $"{Main}/getCustomer";

            public const string getsalessummary = $"{Main}/getsalessummary";
            public const string DaySummary = $"{Main}/daysummary"; 

            public const string CanlSales = $"{Main}/Cancel";

        }
        public class PurchaseEnquiry
        {
            public const string Main = $"{Base}/PurchaseEnquiry";
            public const string SavePUE = $"{Main}/savePUE";
            public const string UpdatePUE = $"{Main}/updatePUE";
            public const string DeletePUE = $"{Main}/deletePUE";
            public const string CancelPUE = $"{Main}/Cancel";
        }

        public class PurchaseRequest
        {
            public const string Main = $"{Base}/purchaseReq";
            public const string SavePuReq = $"{Main}/save";
            public const string UpdatePuReq = $"{Main}/update";
            public const string DeletePuReq = $"{Main}/delete";
            public const string CancelPuReq = $"{Main}/Cancel";
        }
        public class PurchaseQuotaion
        {
            public const string Main = $"{Base}/purchaseQut";
            public const string SavePuQut = $"{Main}/save";
            public const string UpdatePuQut = $"{Main}/update";
            public const string DeletePuQut = $"{Main}/delete";
            public const string CancelpuQut = $"{Main}/Cancel";
        }
        public class InternationalPurchase
        {
            public const string Main = $"{Base}/IntPurchase";
            public const string SaveInpurchase = $"{Main}/save";
            public const string UpdateInpurchase = $"{Main}/update";
            public const string FillInpurchasebyid = $"{Main}/fillbyid";
            public const string FillInpurchase = $"{Main}/fill";
            public const string DelInpurchase = $"{Main}/delete";
            public const string fillitems = $"{Main}/fillitems";
            public const string GetData = $"{Main}/getdata";
            public const string CancelInp = $"{Main}/Cancel";
        }
        public class GoodsInTransit
        {
            public const string Main = $"{Base}/goodsInTransit";
            public const string SaveGIT = $"{Main}/save";
            public const string UpdateGIT = $"{Main}/update";
            public const string DeleteGIT = $"{Main}/delete";
            public const string CancelGit = $"{Main}/Cancel";
        }
        public class SalesReturn
        {
            public const string Main = $"{Base}/salesReturn";
            public const string SaveSalesReturn = $"{Main}/saveSalesRtn";
            public const string UpdateSalesReturn = $"{Main}/updateSalesRtn";
            public const string DelSalesReturn = $"{Main}/delsalesRtn";
            public const string CancelsalesRtn = $"{Main}/Cancel";
        }
        public class PurchaseReturn
        {
            public const string Main = $"{Base}/PurchaseReturn";
            public const string SavePurchaseRtn = $"{Main}/savePR";
            public const string UpdatePurchaseRtn = $"{Main}/updatePR";
            public const string DeletePurchaseRtn = $"{Main}/deletePR";
            public const string CancelPurchaseRtn = $"{Main}/Cancel";
        }
        public class MaterialRequest
        {
            public const string Main = $"{Base}/materialReq";
            public const string GetData = $"{Main}/getdata";
            public const string GetFromWH = $"{Main}/fromWh";
            public const string fillitems = $"{Main}/fillitems";
            public const string Fill = $"{Main}/fill";
            public const string FillById = $"{Main}/fillById";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string getSettings = $"{Main}/getSettings";
            public const string sizeMasterPopup = $"{Main}/sizeMasterPopup";
            public const string findQty = $"{Main}/findQty";
            public const string latVoucherdate = $"{Main}/latVoucherdate";
        }
        public class DeliveryIn
        {
            public const string Main = $"{Base}/deliveryIn";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string Cancel = $"{Main}/Cancel";
        }

        public class MaterialReceipt
        {

            public const string Main = $"{Base}/materialReceipt";
            public const string GetData = $"{Main}/getdata";
            public const string GetFromWH = $"{Main}/fromWh";
            public const string fillitems = $"{Main}/fillitems";
            public const string FillMaster = $"{Main}/fillmaster";
            public const string FillAccount = $"{Main}/fillAccount";
            public const string FillById = $"{Main}/fillById";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";
            public const string CancelMatr = $"{Main}/cancel";
            public const string getSettings = $"{Main}/getSettings";
            public const string sizeMasterPopup = $"{Main}/sizeMasterPopup";
            public const string findQty = $"{Main}/findQty";
            public const string latVoucherdate = $"{Main}/latVoucherdate";
            public const string marPrice = $"{Main}/marPrice";
        }
    }
}
