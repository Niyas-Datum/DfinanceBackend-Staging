

using FluentValidation;
namespace Dfinance.DataModels.Validation
{
    public class ItemValidation:AbstractValidator<ItemMasterDto>
    {
        public ItemValidation()
        {
            RuleFor(i=>i.ItemCode).NotNull().NotEmpty().WithMessage("ItemCode is Mandatory");
            RuleFor(i => i.ItemName).NotNull().NotEmpty().WithMessage("ItemName is Mandatory");
            RuleFor(i => i.Unit.Unit).NotNull().NotEmpty().WithMessage("Unit is Mandatory");
            RuleFor(i => i.Category.ID).NotNull().NotEmpty().WithMessage("Category is Mandatory");
           
        }
    }
}
