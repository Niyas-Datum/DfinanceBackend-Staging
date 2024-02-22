using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.Finance
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
        public int PrimaryVoucherId { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Active is required")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string? Code { get; set; }
        public byte? DevCode { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? Numbering { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set; }
        public int? RowType { get; set; }
        [Required(ErrorMessage = "ApprovalRequired is required")]
        public bool? ApprovalRequired { get; set; }
        [Required(ErrorMessage = "WorkflowDays is required")]
        public int? WorkflowDays { get; set; }
        [Required(ErrorMessage = "ApprovalDays is required")]
        public int? ApprovalDays { get; set; }
        public bool? InventoryUpdate { get; set; }
        public byte? ModuleType { get; set; }
        [Required(ErrorMessage = "ReportPath is required")]
        public string? ReportPath { get; set; }
        public string? Nature { get; set; }
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
