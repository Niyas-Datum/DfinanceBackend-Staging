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
            public const string Accountlist = $"{Main}/getall";
            public const string AccountsById = $"{Main}/getbyid";
            public const string SubGroup = $"{Main}/accsubgrps";
            public const string AccountGroup = $"{Main}/accgrps";
            public const string AccountCategory = $"{Main}/acccat";
            public const string DeleteAccount = $"{Main}/delacc";
            public const string Accounts = $"{Main}/getacc";
            public const string AccountsGroup = $"{Main}/getaccgrp";
            public const string Accountspopup = $"{Main}/accpopup";

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
            public const string popup = $"{Main}/popup";

            public const string dropDown = $"{Main}/dropDown";

            //   public const string SaveVouchers = $"{Main}/save";

            public const string UpdateVouchers = $"{Main}/update";

            // public const string DeleteVouchers = $"{Main}/delete";

            public const string FillPrimaryVoucherName = $"{Main}/getnamebyid";

            public const string SaveVoucherNumbering = $"{Main}/savenumbering";

            public const string UpdateVoucherNumbering = $"{Main}/updatenumbering";

            public const string DeleteVoucherNumbering = $"{Main}/delnumbering"; 
            public const string GetVoucherHistory = $"{Main}/getvochhisy";

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
        public static class PaymentVoucher
        {
            public const string Main = $"{Base}/paymentvou";
            public const string FillAccCode = $"{Main}/Accpopup";
            public const string Getsettings = $"{Main}/getsettings";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";



        }
        public static class ReceiptVoucher
        {
            public const string Main = $"{Base}/receiptvou";
            public const string FillVoucher = $"{Main}/fill";
            public const string FillMaster = $"{Main}/fillmaster";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";


        }
        public static class Budgeting
        {
            public const string Main = $"{Base}/budgeting";
            public const string fillMaster = $"{Main}/fillMaster";
            public const string accPopup = $"{Main}/accPopup";
            public const string plBalsheet = $"{Main}/plBalsheet";
            public const string Save = $"{Main}/save";
            public const string Delete = $"{Main}/delete";
        }
        public static class BudReg
        {
            public const string Main = $"{Base}/budgetReg";
            public const string fill = $"{Main}/fill";
            public const string budPopup = $"{Main}/budPopup";
        }
        public static class BudMonthwise
        {
            public const string Main = $"{Base}/budgetMonthwise";
            public const string fill = $"{Main}/fill";
        }
        public static class DayBook
        {
            public const string Main = $"{Base}/daybook";
            public const string voucherUser = $"{Main}/voucher&User";
            public const string fill = $"{Main}/fill";
        }
        public static class BranchAccounts
        {
            public const string Main = $"{Base}/branchaccounts";
            public const string FillbrAcc = $"{Main}/fillBrAcc";
            public const string update = $"{Main}/update";


        }
        public static class ContraVoucher
        {
            public const string Main = $"{Base}/contravou";
            public const string Fill = $"{Main}/fillAccCode";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";


        }
        public static class AccountReconciliation
        {
            public const string Main = $"{Base}/Accountrec";
            public const string AccPopup = $"{Main}/fillAccpop";
            public const string FillAccountReconcilation = $"{Main}/fillAccReco";
            public const string UpdateAccountReconcilation = $"{Main}/updateAccReco";

        }
        public static class OpeningVoucher
        {
            public const string Main = $"{Base}/Openingvou";
            public const string Save = $"{Main}/save";
            public const string Update = $"{Main}/update";
            public const string Delete = $"{Main}/delete";
        }


            public static class FinStmt
            {
                public const string Main = $"{Base}/FinStmt";
                public const string finStmt = $"{Main}/finStmt";
                public const string accStmt = $"{Main}/accStmt";
                public const string billWiseStmt = $"{Main}/billWiseStmt";
                public const string balSheetStmt = $"{Main}/balSheetStmt";
                public const string consolMonth = $"{Main}/consolMonth";
                public const string partyOutstd = $"{Main}/partyOutstd";
                public const string salesManCol = $"{Main}/salesManCol";
                public const string CrDrBal = $"{Main}/CrDrBal";
                public const string profitLoss = $"{Main}/profitLoss";
                public const string cashFlow = $"{Main}/cashFlow";

                public const string costCentrRep = $"{Main}/costCentrRep";
                public const string accBrkup = $"{Main}/accBrkup";
            }
            public static class AccConfig
            {
                public const string Main = $"{Base}/AccConfig";
                public const string fill = $"{Main}/fill";
                public const string update = $"{Main}/update";
            }



            public static class PdcClearing
            {
                public const string Main = $"{Base}/pdcclearing";
                public const string Fill = $"{Main}/fillCheqdet";
                public const string Save = $"{Main}/save";
                public const string Update = $"{Main}/update";
                public const string Delete = $"{Main}/delete";

            }
            public static class CreditNote
            {
                public const string Main = $"{Base}/creditnote";
                public const string Fill = $"{Main}/fill";
                public const string SaveDebitCredit = $"{Main}/save";
                public const string UpdateDebitCredit = $"{Main}/update";
                public const string DeleteDebitCredit = $"{Main}/delete";
                public const string Cancel = $"{Main}/cancel";



            }

        public static class JournalVoucher
        {
            public const string Main = $"{Base}/journalVoucher";
            public const string fillAcc = $"{Main}/fillAcc";
            public const string save = $"{Main}/save";
            public const string update = $"{Main}/update";
        }
        public static class ChequeRegister
        {
            public const string Main = $"{Base}/chequeregister";
            public const string FillCheqreg = $"{Main}/FCheqreg";
        }
        public static class CustomerRegister
        {
            public const string Main = $"{Base}/customerregister";
            public const string FillCusreg = $"{Main}/Cureg";
        }
        public static class AccountRegister
        {
            public const string Main = $"{Base}/accountregister";
            public const string FillAccreg = $"{Main}/Accreg";
            public const string PopupAccgr = $"{Main}/ACCgrpop";
            public const string PopupSubgr = $"{Main}/Subgrpop";
            public const string PopupPar = $"{Main}/Parentpop";

        }
        public static class CloseVoucher
        {
            public const string Main = $"{Base}/closeVouch";
            public const string LoadData = $"{Main}/LD";
            public const string Fill = $"{Main}/fill";
            public const string Apply = $"{Main}/update";
        }
    }
}
