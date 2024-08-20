using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class InventoryRegisterDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateUpto { get; set; }
        public int? BranchID { get; set; } 
        public int? BasicVTypeID { get; set; }
        public int? VTypeID { get; set; } = null;
        public bool Detailed { get; set; } = false;
        public bool Inventory { get; set; } = false;
        public bool Columnar { get; set; } = false;
        public bool GroupItem { get; set; } = false;
        public string Criteria { get; set; } = "";
        public int? AccountID { get; set; } = null;
        public int? PaymentTypeID { get; set; } = null;
        public int? ItemID { get; set; } = null;
        public int? CounterID { get; set; } = null;
        public string PartyInvNo { get; set; } = "";
        public string BatchNo { get; set; } = "";
        public int? UserID { get; set; } = null;
        public int? StaffID { get; set; } = null;
        public int? AreaID { get; set; } = null;
    }
}
