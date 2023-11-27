using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class SpDesignationMasterG
    {
        public int? ID { get; set; }
        public string? Name { get; set; }

    }
    public class SpDesignationMasterByIdG
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CreatedBranchID { get; set; }
        public string Company { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
    }

}
