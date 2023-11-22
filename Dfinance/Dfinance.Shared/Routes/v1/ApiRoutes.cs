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
            public const string Main = $"{Base}/cmp";

            public const string GetAll = $"{Main}/all";
        }
        public static class Branch
        {
            public const string Main = $"{Base}/branch";

            public const string GetAllBranch = $"{Main}/all";

            public const string FillAllBranch = $"{Main}/FillAllBranch";

            public const string SaveBranch = $"{Main}/save";

            public const string UpdateBranch = $"{Main}/update/{{Id}}";

            public const string Delete = $"{Main}/delete/{{Id}}";

            public const string GetAllById = $"{Main}/GetAllById/{{Id}}";
        }
        public static class Country
        {
            public const string Main = $"{Base}/country";

            public const string GetAll = $"{Main}/DropDown";
        }
       
        public static class DepartmentType
        {
            public const string Main = $"{Base}/dept";

            public const string GetAll = $"{Main}/FillAllDepartment";
            public const string FillDepartmentTypesById= $"{Main}/FillDepartmentTypesById";

            public const string AddDepartmentTypes = $"{Main}/add";

            public const string UpdateDepartmentTypes = $"{Main}/update/{{Id}}";

            public const string DeleteDepartmentTypes = $"{Main}/delete/{{Id}}";

        }
        public static class User
        {
            public const string Main = $"{Base}/user";
            public const string GetAllUser = $"{Main}/DropDown";
            public const string GetUserById = $"{Main}/GetAllUserById";
            public const string AddUser = $"{Main}/add";
            public const string UpdateUser = $"{Main}/update/{{Id}}";
            public const string DeleteUserRight = $"{Main}/deleteuserright/{{Id}}";
            public const string DeleteBranchDetails = $"{Main}/deletebranchdetails/{{Id}}";
            public const string DeleteUser = $"{Main}/deleteuser/{{Id}}";
        }
        public static class Designation
        {
            public const string Main = $"{Base}/dept";

            public const string GetAllDesignation = $"{Main}/FillAllDesignation";

            public const string GetAllDesignationById = $"{Main}/GetAllDesignationById";

            public const string AddDesignation = $"{Main}/add";

            public const string UpdateDesignation = $"{Main}/update/{{Id}}";

            public const string DeleteDesignation = $"{Main}/delete/{{Id}}";
        }
        public static class CostCategory
        {
            public const string Main = $"{Base}/CostCategory";

            public const string SaveCostCategory = $"{Main}/Save";

            public const string UpdateCostCategory = $"{Main}/update/{{Id}}";

            public const string DeleteCostCategory= $"{Main}/Delete/{{Id}}";

            public const string FillCostCategory = $"{Main}/Get";

            public const string FillCostCategoryById = $"{Main}/GetById";

        }
        public static class CostCentre
        {
            public const string Main = $"{Base}/CostCentre";

            public const string SaveCostCentre = $"{Main}/Save";

            public const string FillCostCentre = $"{Main}/Get";

            public const string FillCostCentreById = $"{Main}/GetById";

            public const string UpdateCostCentre = $"{Main}/Update/{{Id}}";

            public const string DeleteCostCentre = $"{Main}/Delete";
        }

        public static class CostCategoryDropDown
        {
            public const string Main = $"{Base}/CostCategoryDropDown";

            public const string GetAll = $"{Main}/Get";
        }

        public static class CostCentreDropDown
        {
            public const string Main = $"{Base}/CostCentreDropDown";

            public const string GetAll = $"{Main}/Get";
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

            public const string GetCategory = $"{Main}/Get";

            public const string GetCategoryById = $"{Main}/GetById";
        }

        public static class  AreaMaster
        {
            public const string Main = $"{Base}/AreaMaster";

            public const string SaveAreaMaster = $"{Main}/Save";

            public const string UpdateAreaMaster = $"{Main}/Update";

            public const string DeleteAreaMaster = $"{Main}/Delete";

            public const string GetAreaMaster = $"{Main}/Get";
            
            public const string GetAreaMasterById = $"{Main}/GetById";
        }
    }
}