

namespace Dfinance.Core.Domain
{
    public class TaxType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int SalePurchaseModeId { get; set; }
        public int? TaxAccountId { get; set; }
        public decimal? SalesPerc { get; set; }
        public decimal? PurchasePerc { get; set; }
        public bool? Active { get; set; }
        public string? Note { get; set; }
        public int? TaxMiscId { get; set; }
        public int? ReceivableAccountId { get; set; }
        public int? PayableAccountId { get; set; }
        public int? SGSTReceivable { get; set; }
        public int? SGSTPayable { get; set; }
        public int? CGSTReceivable { get; set; }
        public int? CGSTPayable { get; set; }
        public int? SGSTReceivableAccountId { get; set; }
        public int? SGSTPayableAccountId { get; set; }
        public int? CGSTReceivableAccountId { get; set; }
        public int? CGSTPayableAccountId { get; set; }
        public decimal? CessPerc { get; set; }
        public int? CessPayable { get; set; }
        public int? CessReceivable { get; set; }
        public bool? IsDefault { get; set; }
    }
}
