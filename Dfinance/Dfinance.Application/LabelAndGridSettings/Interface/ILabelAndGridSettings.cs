using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.LabelAndGridSettings.Interface
{
    public interface ILabelAndGridSettings
    {
        CommonResponse FillFormLabelSettings();
        CommonResponse FillGridSettings();
        CommonResponse UpdateLabel(List<LabelDto> labelDto, string password);
        CommonResponse UpdateGrid(List<GridDto> gridDto, string password);
        CommonResponse labelGridpopup();
    }
}
