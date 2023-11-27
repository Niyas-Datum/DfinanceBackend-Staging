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
        public static class Country
        {
            public const string Main = $"{Base}/country";

            public const string GetAll = $"{Main}/DropDown";
        }
        public static class Department
        { 

          public const string Main = $"{Base}/dept";
            public const string DropDown = $"{Main}/DropDownDepartemnt";
            public const string FillAllDepartment = $"{Main}/FillAllDepartment";

        public const string FillDepartmentById = $"{Main}/FillDepartmentById";
         }
            public static class DepartmentType
        {
            public const string Main = $"{Base}/depttyp";

            public const string FillAll = $"{Main}/FillAllDepartmentTypes";

            public const string FillDepartmentTypesById= $"{Main}/FillDepartmentTypesById";

            public const string SaveDepartmentTypes = $"{Main}/Save";

            public const string UpdateDepartmentTypes = $"{Main}/update/{{Id}}";

            public const string DeleteDepartmentTypes = $"{Main}/delete/{{Id}}";

        }
        public static class User
        {
            public const string Main = $"{Base}/user";

            public const string UserDropDown = $"{Main}/UserDropDown";
            public const string FillUser= $"{Main}/FillUser";

            public const string FillUserById = $"{Main}/FillUserById";

            public const string SaveUser = $"{Main}/Save";

            public const string UpdateUser = $"{Main}/update/{{Id}}";

            public const string DeleteUserRight = $"{Main}/deleteuserright/{{Id}}";

            public const string DeleteBranchDetails = $"{Main}/deletebranchdetails/{{Id}}";

            public const string DeleteUser = $"{Main}/deleteuser/{{Id}}";
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

            public const string DropDown = $"{Base}/DropDown";

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

     
        public static class StatusDropDown
        {
            public const string Main = $"{Base}/StatusDropDown";

            public const string GetAll = $"{Main}/Get";
        }

        public static class Category
        {
            public const string Main = $"{Base}/Category";

            public const string SaveCategory = $"{Main}/Save";

            public const string UpdateCategory = $"{Main}/Update";

            public const string DeleteCategory = $"{Main}/Delete";

            public const string FillCategory = $"{Main}/Fill";

            public const string FillCategoryById = $"{Main}/FillById";
        }

        public static class  AreaMaster
        {
            public const string Main = $"{Base}/AreaMaster";

            public const string SaveAreaMaster = $"{Main}/Save";

            public const string UpdateAreaMaster = $"{Main}/Update";

            public const string DeleteAreaMaster = $"{Main}/Delete";

            public const string FillAreaMaster = $"{Main}/Fill";
            
            public const string FillAreaMasterById = $"{Main}/FillById";
        }
    }
}