using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Inventory.Purchase
{
    public class PurchaseReportDto
    {
        public string ViewBy { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public PopUpDto? BaseType { get; set; }
        public PopUpDto? VoucherType { get; set; }
        public PopUpDto? customerSupplier { get; set; }
        public PopUpDto? Item { get; set; }
        public PopUpDto? Staff { get; set; }
        public PopUpDto? Area { get; set; }
        public PopUpDto? PaymentType { get; set; }
        public PopUpDto? Counter { get; set; }
        public PopUpDto? User { get; set; }
        public PopUpDto? InvoiceNo { get; set; }
        public PopUpDto? BatchNo { get; set; }
        //public PopUpDto? OrderNo { get; set; }
        public DropdownDto? Branch { get; set; }

        public bool? Detailed { get; set; } = false;
        public bool? Columnar { get; set; } = false;
        public bool? GroupItem { get; set; } = false;
        public bool? Inventory { get; set; } = false;

    }
}
