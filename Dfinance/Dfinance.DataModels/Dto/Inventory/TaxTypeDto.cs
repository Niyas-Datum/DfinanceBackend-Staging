using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class TaxTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DropdownDto Type { get; set; }
        public decimal PurchasePerc { get; set; }
        public decimal SalesPerc { get; set; }
        public DropdownDto ReceivableAccount { get; set; }
        public DropdownDto PayableAccount { get; set; }
        public DropdownDto Sales_Pur_Mode { get; set; }
        public DropdownDto TaxType { get; set; }
        public DropdownDto SGSTReceivable { get; set; }
        public DropdownDto CGSTReceivable { get; set; }
        public DropdownDto SGSTPayable { get; set; }
        public DropdownDto CGSTPayable { get; set; }
        public decimal Cess_Perc { get; set; }
        public DropdownDto CessReceivable { get; set; }
        public DropdownDto CessPayable { get; set; }
        public string Description { get; set; }
        public bool Active {  get; set; }
        public bool Default { get; set; }   

    }
}
