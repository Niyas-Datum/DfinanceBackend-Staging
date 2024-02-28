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
    }
}
