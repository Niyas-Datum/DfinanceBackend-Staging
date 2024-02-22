using Dfiance.Hr.Employees.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfiance.Hr.Employees
{
    public class HrEmployeeService : IHrEmployeeService
    {
        private  readonly DFCoreContext _context;
        public HrEmployeeService(DFCoreContext context)
        {
            _context = context;
        }
        ////******************************************************SalesMan*************************************************************
        public CommonResponse GetSalesMan()
        {
            try
            {
                string criteria = "FillSalesman";
                var result = _context.ReadView.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
    }
}

