
using System.ComponentModel.DataAnnotations;

namespace Dfinance.Core.Domain
{
    public class UnitMaster
    {
        [Key]
        public string Unit { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
        public bool IsComplex { get; set; }
        public string? BasicUnit { get; set; }
        public bool AllowDelete { get; set; }
        public int? Precision { get; set; }
        public decimal? Factor1 { get; set; }
        public bool? Active { get; set; }
        public string? ArabicName { get; set; }

        //relationship with ItemUnits
        public virtual ICollection<ItemUnits> UnitsItem { get; set; } = new List<ItemUnits>();//Unit
        public virtual ICollection<ItemUnits> BasicUnitItem { get; set; } = new List<ItemUnits>();//BasicUnit

    }
}
