namespace Dfinance.Shared.Routes.v1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = $"{Root}/{Version}";

        public static class Authroutes
        {
            public const string login = $"{Base}/auth";
        }
        public static class Company
        {
            public const string Main = $"{Base}/Company";

            public const string GetAll = $"{Main}/CmpLogin";
        }
        public static class Branch
        {
            public const string Main = $"{Base}/branch";

            public const string GetBranchDropDown = $"{Main}/DropDown";

            public const string FillAllBranch = $"{Main}/FillAllBranch";

            public const string SaveBranch = $"{Main}/save";

            public const string UpdateBranch = $"{Main}/update/{{Id}}";

            public const string Delete = $"{Main}/delete/{{Id}}";

            public const string FillBranchById = $"{Main}/FillBranchById/{{Id}}";
        }
        public static class Miscellaneous
        {
            public const string Main = $"{Base}/miscellaneous";

            public const string GetDropdown = $"{Main}/DropDown";

            public const string GetPopup = $"{Main}/popup";

        }
       
        public static class Department
        { 

          public const string Main = $"{Base}/dept";
            public const string DropDown = $"{Main}/DropDownDepartemnt";
            public const string FillAllDepartment = $"{Main}/FillAllDepartment";
            public const string FillDepartmentById = $"{Main}/FillDepartmentById";
            public const string SaveDepartmentTypes = $"{Main}/Save";
            public const string UpdateDepartmentType = $"{Main}/Update";
            public const string DeleteDepartmentTypes = $"{Main}/delete/{{Id}}";
            public const string DeptPopup= $"{Main}/deptpopup";
        }
        public static class User
        {
            public const string Main = $"{Base}/user";

            public const string UserDropDown = $"{Main}/UserDropDown";
            public const string FillUser= $"{Main}/FillUser";
            public const string FillPettyCash = $"{Main}/fillPettyCash";
            public const string FillUserById = $"{Main}/FillUserById";

            public const string SaveUser = $"{Main}/Save";

            public const string UpdateUser = $"{Main}/update";
            public const string DeleteUser = $"{Main}/deleteuser";
            public const string FillRole = $"{Main}/fillrole";
            public const string GetRole = $"{Main}/getrole";
        }
        public static class Designation
        {
            public const string Main = $"{Base}/Desg";

            public const string FillAllDesignation = $"{Main}/FillAllDesignation";

            public const string FillDesignationById = $"{Main}/FillDesignationById";

            public const string SaveDesignation = $"{Main}/Save";

            public const string UpdateDesignation = $"{Main}/Update/{{Id}}";

            public const string DeleteDesignation = $"{Main}/Delete/{{Id}}";
        }
        public static class CostCategory
        {
            public const string Main = $"{Base}/CostCategory";

            public const string SaveCostCategory = $"{Main}/Save";

            public const string UpdateCostCategory = $"{Main}/Update/{{Id}}";

            public const string DeleteCostCategory= $"{Main}/Delete/{{Id}}";

            public const string FillCostCategory = $"{Main}/Fill";

            public const string FillCostCategoryById = $"{Main}/FillById";

            public const string DropDown = $"{Main}/DropDown";

        }
        public static class CostCentre
        {
            public const string Main = $"{Base}/CostCentre";

            public const string SaveCostCentre = $"{Main}/Save";

            public const string FillCostCentre = $"{Main}/Fill";

            public const string FillCostCentreById = $"{Main}/FillById";

            public const string UpdateCostCentre = $"{Main}/Update/{{Id}}";

            public const string DeleteCostCentre = $"{Main}/Delete";

            public const string DropDown = $"{Base}/DropDown";

            public const string FillPopUp = $"{Main}/ClientPopUp";

        }

     
      

        public static class Category
        {
            public const string Main = $"{Base}/Category";

            public const string SaveCategory = $"{Main}/Save";

            public const string UpdateCategory = $"{Main}/Update";

            public const string DeleteCategory = $"{Main}/Delete";

            public const string FillCategory = $"{Main}/Fill";

            public const string FillCategoryById = $"{Main}/FillById";

            public const string NextCode = $"{Main}/nextCode";
        }

        public static class  AreaMaster
        {
            public const string Main = $"{Base}/AreaMaster";

            public const string SaveAreaMaster = $"{Main}/Save";

            public const string UpdateAreaMaster = $"{Main}/Update";

            public const string DeleteAreaMaster = $"{Main}/Delete";

            public const string FillAreaMaster = $"{Main}/Fill";
            
            public const string FillAreaMasterById = $"{Main}/FillById";
            public const string PopUp = $"{Main}/poparea";
        }

        public static class CategoryType
        {
            public const string Main = $"{Base}/CategoryType";            

            public const string GetNextCode = $"{Main}/GetNextCode";

            public const string FillCategoryType = $"{Main}/Fill";

            public const string FillCategoryTypeById = $"{Main}/FillById";

            public const string SaveCategoryType = $"{Main}/Save";

            public const string UpdateCategoryType = $"{Main}/Update";

            public const string DeleteCategoryType = $"{Main}/Delete";
			public const string GetCatType = $"{Main}/getcattype";
            
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
        public static class MaSettings
        {
            public const string Main = $"{Base}/sett";

            public const string FillMaster = $"{Main}/getall";

            public const string FillByID = $"{Main}/getbyid";

            public const string SaveSettings = $"{Main}/save";

            public const string UpdateSettings = $"{Main}/update";

            public const string DeleteSettings = $"{Main}/delete";

            public const string KeyValue = $"{Main}/KeyValue";
            public const string GetAllSettings = $"{Main}/getallsettings";

        }
		  public static class UserTrack
        {
            public const string Main = $"{Base}/UserTrack";
            public const string FillUserTrack = $"{Main}/FillUserTrack";
        }
        public static class Password
        {
            public const string Main = $"{Base}/pwd";
            public const string GetPassword = $"{Main}/varify";
        }
        public static class Parties
        {
            public const string Main = $"{Base}/cussupp";
            public const string SaveCustmsupp = $"{Main}/save";
            public const string GetCode = $"{Main}/getcode";
            public const string GetCategory = $"{Main}/getcat";
            public const string supplier = $"{Main}/getsupplier";
            public const string GetType = $"{Main}/gettype";
            public const string update = $"{Main}/update"; 
            public const string FillParty = $"{Main}/fillparty";
            public const string FillPartyById = $"{Main}/FillPartyById";
            public const string Delete = $"{Main}/del";
        }
        public static class CustomerDetails
        {
            public const string Main = $"{Base}/cust";
            public const string GetPriceCategory = $"{Main}/getpricecatg";
            public const string CustomerCategories = $"{Main}/getcategory";
            public const string GetCommodity = $"{Main}/getcomdity";
            public const string customer = $"{Main}/getcustomer";
            public const string CreditDropdown = $"{Main}/CreditDropdown";
            
        }
        public static class HR
        {
            public const string Main = $"{Base}/Hr";
            public const string Salesman = $"{Main}/getsalesman";
        }

        public static class ItemMaster
        {
            public const string Main = $"{Base}/ItemMaster";

            public const string FillMaster = $"{Main}/fillmaster";

            public const string FillById = $"{Main}/FillById";

            public const string GetNextCode = $"{Main}/NxtItemCode";          

            public const string ParentPopup = $"{Main}/FillParent";

            public const string SaveItem = $"{Main}/additem";

            public const string UpdateItem = $"{Main}/updateitem";

            public const string DeletItem = $"{Main}/deleteitem";

            public const string Barcode = $"{Main}/GenBarcode";

            public const string TaxDropdown = $"{Main}/TaxDropdown";
            public const string Itemsearch = $"{Main}/ itemsearch"; 
            public const string ItemRegister = $"{Main}/ itemregister";
        }
        public static class ItemUnits
        {
            public const string Main = $"{Base}/ItemUnits";

            public const string FillItemUnits = $"{Main}/FillItemUnits";

            public const string SaveItemUnit = $"{Main}/additemunit";

            public const string UpdateItemUnit = $"{Main}/updateitemunit";

            public const string DeletItemUnit = $"{Main}/deleteitemunit";
            public const string FillBranchwiseUnits = $"{Main}/branchwiseunits";
        }

      

        public static class TaxType
        {
            public const string Main = $"{Base}/TaxType";

            public const string TaxTypeDropDown = $"{Main}/FillTaxType";
        }

        public static class Role
        {
            public const string Main = $"{Base}/Role";
            public const string FillRole = $"{Main}/fillrole";
            public const string FillRoleRight = $"{Main}/fillroleright";
            public const string FillRoleAndRight = $"{Main}/fillroleandright";
            public const string Saverole = $"{Main}/Saverole";
            public const string UpdateRole = $"{Main}/UpdateRole";
            public const string DeleteRole = $"{Main}/Deleterole";
        }
        public static class LabelGrid
        {
            public const string Main = $"{Base}/LabelGrid";
            public const string Getlabel = $"{Main}/Getlabel";
            public const string Getgrid = $"{Main}/Getgrid";
            public const string UpdateLabel = $"{Main}/UpdateLabel";
            public const string Updategrid = $"{Main}/Updategrid";
            public const string labelGridpopup = $"{Main}/labelGridpopup";

        }
        public static class RecallVoucher
        {
            public const string Main = $"{Base}/RecallVch";
            public const string GatData = $"{Main}/getData";
            public const string GetCancelVch = $"{Main}/canclVch";
            public const string UpdateVch = $"{Main}/updateVch";
        }
    }
}