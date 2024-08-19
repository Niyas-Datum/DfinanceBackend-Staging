using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.LabelAndGridSettings.Interface
{
    public interface ILabelAndGridSettings
    {
        CommonResponse FillFormLabelSettings();
        CommonResponse FillGridSettings();
        CommonResponse FormNamePopup();
        CommonResponse PagePopUp();
        CommonResponse SaveAndUpdateLabel(List<LabelDto> labelDto, string password);
        CommonResponse SaveAndUpdateGrid(List<GridDto> gridDto, string password);
        CommonResponse labelGridpopup();
        CommonResponse GetGridByPageId(int pageId);
        CommonResponse GetLabelByPageId(int pageId);
    }
}
