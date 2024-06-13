using Dfinance.DataModels.Dto.Common;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.Finance
{
    public class ChartOfAccountsDto
    {
        [Required(ErrorMessage = "Group is mandatory!!")]
        public DropDownDtoName? Group { get; set; }//DropDown 
        [Required(ErrorMessage = "SubGroup is mandatory!!")]
        public DropDownDtoName? SubGroup { get; set; }//DropDown
        public DropDownDtoName? AccountCategory { get; set; }//DropDown
        [Required(ErrorMessage = "AccountName is mandatory!!")]
        public string AccountName {  get; set; }
        [Required(ErrorMessage = "AccountCode is mandatory!!")]
        public string AccountCode { get; set; } //AutoGen
        public string? Narration { get; set; } = null;
        public bool? MaintainBillwise { get; set; } 
        public  bool Active { get; set;}
        public bool? PreventExtraPay {  get; set; }
        public bool MaintainIteamWise {  get; set; }
        public bool? TrackCollection { get;set; }
        public bool? MaintainCostCentre {  get; set; }
        public string? AlternateName { get; set; }
        public bool IsGroup { get; set; }

    }
    public class AccountDto
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int ID { get; set; }
    }

}
