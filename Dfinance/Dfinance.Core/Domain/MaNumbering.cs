namespace Dfinance.Core.Domain
{
    public class MaNumbering
    {
        public int Id { get; set; }

        public int? StartingNumber { get; set; }

        public int? MaximumDegits { get; set; }

        public bool? Prefillwithzero { get; set; }

        public int? Renewedby { get; set; }

        public int? Prefix { get; set; }

        public string? PrefixValue { get; set; }

        public int? Suffix { get; set; }

        public string? SuffixValue { get; set; }

        public bool? Editable { get; set; }

        public virtual ICollection<Voucher>? FiMaVouchers { get; set; }

        //public virtual MaRenewal? RenewedbyNavigation { get; set; }
    }
}
