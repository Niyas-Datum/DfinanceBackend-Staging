using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class FillDocTypeMasterView
    {
        public int? ID {  get; set; }
        public string? Description {  get; set; }
        public string? Directory {  get; set; }
    }
    public class FillDocTypeByIdView
    {
        public int? ID { get; set; }
        public string? Description {  get; set; }
        public bool? IsLCDoc {  get; set; }
        public bool? IsLCMandatoryDoc { get; set; }
        public bool? IsPODoc { get; set; }
        public string? Directory { get; set; }
        public string? PIRDescription {  get; set; }
        public int? DocRowType { get; set; }
        public string? DocRowTypeName {  get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte? ActiveFlag { get; set; }

    }
}
