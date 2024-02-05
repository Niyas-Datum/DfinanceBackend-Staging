using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto.Finance
{
    public class FiMaVoucherDto
    {
        public VoucherDto voucherDto { get; set; }
        public NumberingDto? numberingDto { get; set; }

    }
    public class VoucherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int PrimaryVoucherName { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
        public string? Code { get; set; }
        public byte? DevCode { get; set; }
        public int? DocumentTypeName { get; set; }
        public int? Numbering { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set; }
        public int? RowType { get; set; }
        public bool? ApprovalRequired { get; set; }
        public int? WorkflowDays { get; set; }
        public int? ApprovalDays { get; set; }
        public bool? InventoryUpdate { get; set; }
        public byte? ModuleType { get; set; }
        public string? ReportPath { get; set; }
        public string? Nature { get; set;}
    }

    public class NumberingDto
    {

        //public int? Id { get; set; }

        [Required(ErrorMessage = "Starting Number is mandatory!!")]
        public int? StartingNumber { get; set; }

        [Required(ErrorMessage = "Maximum digits is mandatory!!")]
        public int? MaximumDegits { get; set; }
    }

   
   
      
}
