using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.General
{
    public class DocumentTypeDto
    {
        public int? Id { get; set; }
        public string DocumentName {  get; set; }//Description
        public string Directory { get; set; }
        public string? PIRDescription { get; set; }
        public DropdownDto? DocEntry {  get; set; }
        public bool? PurchaseOrder { get; set; }
        public bool? LC {  get; set; }
        public bool? MandatoryLC { get; set; }
        
    }
}
