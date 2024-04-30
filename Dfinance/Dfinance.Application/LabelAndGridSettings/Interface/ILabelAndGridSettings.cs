using Dfinance.Shared.Domain;

namespace Dfinance.Application.LabelAndGridSettings.Interface
{
    public interface ILabelAndGridSettings
    {
        CommonResponse FillFormLabelSettings();
        CommonResponse FillGridSettings();
    }
}
