using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class ItemUnits
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Unit { get; set; } = null!;
        public string BasicUnit { get; set; } = null!;
        public decimal Factor { get; set; }
        public bool? Active { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? PurchaseRate { get; set; }
        public string? BarCode { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? DiscountPrice { get; set; }//WholeSalePrice2
        public decimal? OtherPrice { get; set; }//RetailPrice2
        public decimal? LowestRate { get; set; }
        public decimal? MRP { get; set; }
        public int? BranchId {  get; set; }

        //relationships
        public virtual ItemMaster Item { get; set; } = null!;//self reference =>parentitem
        public virtual UnitMaster Units { get; set; } = null!;//relationship with unitmaster
        public virtual UnitMaster BasicUnits { get; set; } = null!;//relationship with unitmaster
        public virtual MaCompany Branch { get; set; } = null!;//relationship with macompany
    }
}
