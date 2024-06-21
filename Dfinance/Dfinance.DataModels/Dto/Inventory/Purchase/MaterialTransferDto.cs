using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Inventory.Purchase
{
    public class MaterialTransferDto
    {
        public int? TransactionId { get; set; }
        public string? VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }

        public string? Description { get; set; }
        public PopUpDto? BranchAccount { get; set; }
        public PopUpDto? Account { get; set; }
        public string? Reference { get; set; }
        public decimal? Total { get; set; }
        public MaterialTransAddDto? materialTransAddDto { get; set; }
        public List<InvTransItemDto> Items { get; set; }
        public List<ReferenceDto>? References { get; set; }
    }
    public class MaterialTransAddDto
    {
        public DropdownDto MainBranch { get; set; }//for material req
        public DropdownDto? SubBranch { get; set; }//for material req
        public DropdownDto? FromWarehouse { get; set; }//for material req
        public DropdownDto? ToWarehouse { get; set; }//for material req
        public string? Terms {  get; set; }
    }
}
