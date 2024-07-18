using Azure;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Finance.Services
{
    public class ChequeRegisterService : IChequeRegister
    {

        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public ChequeRegisterService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

             
        
        /// <summary>
        /// Fin=>Register=>Chequesreg
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillChequeRegister()
        {
            try
            {
                

                int branchId = _authService.GetBranchId().Value;               
                 
                var criteria = "FillChequeRegister";
                var result = _context.ChequeregViews
                     .FromSqlRaw($"Exec ChequeRegisterSp @Criteria='{criteria}',@BranchID='{branchId}'").ToList();
                         
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}





