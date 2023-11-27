using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
   public class spDepartmentTypesGetById
    {
        public int ID {  get; set; }
        public string Department { get; set; }
        public int CreatedBranchID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
    }
    public class SpReDepartmentTypeFillAllDepartment
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class spDepartmentTypesFillAllDepartmentTypes
    {
        public int Id { get; set; }
        public string Department { get; set; }
    }


    public class spMaDepartmentsFillAllDepartment
    {
        public int ID { get; set; }
        public string DepartmentType { get; set; }
        public string Company { get; set; }
    }
    public class spMaDepartmentsFillDepartmentById
    {
        public int ID { get; set; }
        public int DepartmentTypeID { get; set; }
        public string DepartmentType { get; set; }
        public int CompanyID { get; set; }
        public string Company { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
    }
}
