using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{

    public class UnitPopupView
    {
        [Key]
        public string Unit { get; set; }
        public string BasicUnit { get; set; }
        public decimal Factor { get; set; }
    }
   
	 public class SpFillUnitMaster
    {
        public string Unit { get; set; }
        public string Description { get; set; }
    }
    public class SpFillByUnit
    {
        [Key]
        public string Unit { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
        public bool IsComplex { get; set; }
        public string? BasicUnit { get; set; }
        public bool AllowDelete { get; set; }
        public int? Precision { get; set; }

        public bool? Active { get; set; }
        public string? ArabicName { get; set; }

    }

   public class DropDownView
    {
        public long ID { get; set; }
        public string Unit { get; set; }
        public decimal Factor { get; set; }
    }

}
