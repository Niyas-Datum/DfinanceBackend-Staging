using Dfinance.DataModels.Dto.Common;
using System.Reflection.PortableExecutable;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class InventoryTransactionsDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }    
        public DropdownDto Mode { get; set; }
        public string? VoucherNo {  get; set; }
        public DropdownDto? VoucherType {  get; set; }   
        public DropdownDto? Account {  get; set; }  
        public DropdownDto? CostCentre { get; set; }
        public string? Narration {  get; set; } 
        public DropdownDto? Machine {  get; set; }
        public DropdownDto? Status { get; set; }    
        public bool? Posted { get; set; }   
        public string? AutoID {  get; set; }

    }
}
