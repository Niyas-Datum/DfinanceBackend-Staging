using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.General
{
    public class PageMenuDto
    {
        public int? Id { get; set; }
        public string? MenuText {  get; set; }
        public DropdownDto? Group {  get; set; }
        public string? AssemblyName {  get; set; }
        public bool? Active { get; set; }
        public string? UrlID {  get; set; }
        public bool? IsFinanceRef {  get; set; }
        public DropdownDto? Module { get; set; }
        public DropDownDtoNature? MenuPermission { get; set; }
        public bool? IsMaximized { get; set; }
        public bool? FrequentlyUsed { get; set; }
        public string? ArabicName { get; set; }
        public string? MenuValue { get; set; }
        public bool? IsPage { get; set; }
        public string? FormName { get; set; }
        public int? MenuOrder { get; set; }
        public string? Url { get; set; }
        public DropdownDto? Voucher { get; set; }
        public string? PageTitle { get; set; }
        public int? MenuLevel { get; set; }
        public bool? MDIParent { get; set; }
        public string? ShortcutKey { get; set; }
        public string? ProcedureName { get; set; }
    }
    public class PageActiveDto
    {
        public int? PageId { get; set; }
        public bool? Active { get; set; }
        public bool? FrequentlyUsed { get; set; }
    }
}
