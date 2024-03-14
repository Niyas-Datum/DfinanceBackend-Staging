using Dfinance.Core.Domain;
using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto
{
    public class FiTransactionAdditionalDto
    {
        public int TransactionId { get; set; }
        public string? PartyInvoiceNo { get; set; }
        public DateTime? PartyDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderNo { get; set; }
        public string? PartyNameandAddress { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DropdownDto TransPortationType { get; set; }
        public int? CreditPeriod { get; set; }
        public PopUpDto? SalesMan { get; set; }
        public DropdownDto? SalesArea { get; set; }
        public decimal? StaffIncentives { get; set; }
        public string? MobileNo { get; set; }
        public PopUpDto? VehicleNo { get; set; }
        public string? Attention { get; set; }
        public string? DespatchNo { get; set; }
        public DateTime? DespatchDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryNote { get; set; }
        public string? PartyName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public PopUpDto? DelivaryLocation { get; set; }
        public string? TermsOfDelivery { get; set; }

    }
}
