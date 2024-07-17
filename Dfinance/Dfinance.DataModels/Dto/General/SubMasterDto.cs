using Dfinance.DataModels.Validation;

namespace Dfinance.DataModels.Dto.General
{
    public class SubMasterDto
    {
        public int Id { get; set; }
        public dropdownkey? Key {  get; set; }
        [NullValidation(ErrorMessage = "Value is Mandatory")]
        public string? Code { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
    }
    public class dropdownkey
    {
        public string? Value {  get; set; }
    }
}
