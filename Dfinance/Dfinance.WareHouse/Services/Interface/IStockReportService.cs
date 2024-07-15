using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Warehouse.Services.Interface
{
    public interface IStockReportService
    {
        CommonResponse GetLoadData();
        CommonResponse FillItemReports(StockRegistration stockRegistration);
    }
}
