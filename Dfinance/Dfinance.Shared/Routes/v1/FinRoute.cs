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
            public const string Accountlist= $"{Main}/getall";
            public const string AccountsById = $"{Main}/getbyid";
            public const string SubGroup = $"{Main}/accsubgrps";
            public const string AccountGroup = $"{Main}/accgrps";
            public const string AccountCategory = $"{Main}/acccat";
            public const string DeleteAccount = $"{Main}/delacc";
            public const string Accounts = $"{Main}/getacc";
            public const string AccountsGroup = $"{Main}/getaccgrp";


        }
        public static class Currency
        {
            public const string Main = $"{Base}/Currency";

            public const string SaveCurrencycode = $"{Main}/Savecc";

            public const string UpdateCurrencycode = $"{Main}/Updatecc";

            public const string DeleteCurrencycode = $"{Main}/Deletecc";

            public const string FillAllCurrencycode = $"{Main}/Fillcc";

            public const string FillCurrencycodeById = $"{Main}/FillByIdcc";

            public const string SaveCurrency = $"{Main}/Save";

            public const string UpdateCurrency = $"{Main}/Update";

            public const string DeleteCurrency = $"{Main}/Delete";

            public const string FillAllCurrency = $"{Main}/Fillc";

            public const string FillCurrencyById = $"{Main}/FillById";
        }
        public static class Voucher
        {
            public const string Main = $"{Base}/vchr";

            public const string FillVouchers = $"{Main}/getall";

         //   public const string SaveVouchers = $"{Main}/save";

            public const string UpdateVouchers = $"{Main}/update";

           // public const string DeleteVouchers = $"{Main}/delete";

            public const string FillPrimaryVoucherName = $"{Main}/getnamebyid";

            public const string SaveVoucherNumbering = $"{Main}/savenumbering";

            public const string UpdateVoucherNumbering = $"{Main}/updatenumbering";

            public const string DeleteVoucherNumbering = $"{Main}/delnumbering";


        }
        public static class FinanceYear
        {
            public const string Main = $"{Base}/year";

            public const string SaveFinanceYear = $"{Main}/save";

            public const string UpdateFinanceYear = $"{Main}/Update";

            public const string DeleteFinanceYear = $"{Main}/Delete";

            public const string FillAllFinanceYear = $"{Main}/getall";

            public const string FillAllFinanceYearById = $"{Main}/getbyid";
        }
		public static class AccountsList
        {
            public const string Main = $"{Base}/acclist";
            public const string SaveAccountsList = $"{Main}/Save";
            public const string FillAccountList = $"{Main}/get";
            public const string FillAccountListByID = $"{Main}/getbyid";
            public const string AccountListPopUP = $"{Main}/PopUp";

        }
        public static class CardMaster
        {
            public const string Main = $"{Base}/CardMaster";
            public const string SaveCardMaster = $"{Main}/SaveCardMaster";
            public const string UpdateCardMaster = $"{Main}/UpdateCardMaster";
            public const string DeleteCardMaster = $"{Main}/DeleteCardMaster";
            public const string FillCardMaster = $"{Main}/FillCardMaster";
            public const string FillMaster = $"{Main}/FillMaster";

        }

    }
}
