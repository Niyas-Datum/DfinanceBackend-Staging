using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Finance
{
    public class CardMasterDto
    {
        public string? Description { get; set; }
        public AccountNamePopUpDto? AccountName { get; set; }
        public double? Commission { get; set; }
        public bool? Default { get; set; }
    }
}
