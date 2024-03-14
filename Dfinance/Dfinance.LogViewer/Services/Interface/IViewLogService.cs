using Dfinance.Shared.Domain;

namespace Dfinance.LogViewer.Services.Interface
{
    public interface IViewLogService
    {
        CommonResponse ViewLogs(DateOnly date, string method = null);
    }
}
