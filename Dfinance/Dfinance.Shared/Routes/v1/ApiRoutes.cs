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

            public const string AddDepartmentTypes = $"{Main}/add";

            public const string UpdateDepartmentTypes = $"{Main}/update/{{Id}}";

            public const string DeleteDepartmentTypes = $"{Main}/delete/{{Id}}";

        }
        public static class Employee
        {
            public const string Main = $"{Base}/Emp";

            public const string AddEmployee = $"{Main}/add";

            public const string GetAll = $"{Main}/DropDown";
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
    }
}