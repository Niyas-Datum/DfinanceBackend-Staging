using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;
using System.Text.Json.Serialization;

namespace Dfinance.DataModels.Dto.Inventory.Purchase
{
    public class PurchaseWithoutTaxDto:InventoryTransactionDto
    {
       
        public PopUpDto Account {  get; set; }
        public AdditionalDto TransactionAdditional {  get; set; }

        private new InvTransactionAdditionalDto? FiTransactionAdditional
        {
            get { return null; }
            set { /* Do nothing */ }
        }

    }
    public class AdditionalDto: InvTransactionAdditionalDto
    {       
        public DropdownDto Type { get; set; }
        public PopUpDto PriceCategory { get; set; }
        public decimal? OtherDiscAmt {  get; set; }

    }
}
