using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.Finance.Statements
{
    public class DayBookService:IDayBookService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<DayBookService> _logger;
        public DayBookService(DFCoreContext context, IAuthService authService, ILogger<DayBookService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }

        public CommonResponse FillVoucherAndUser()
        {
           try
            {
                var vouchers = _context.DropDownViewName.FromSqlRaw("Exec DropDownListSP @Criteria='FillVouchers'").ToList();
                var users = _context.MaEmployees.Where(u => u.Active == true).Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.FirstName,
                    u.LastName,
                    u.EmailId,
                    u.MobileNumber
                }).ToList();
                return CommonResponse.Ok(new { VoucherType = vouchers, Users = users });
            }
           catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse FillDayBook(DayBookDto dayBookDto,int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Save Budget");
            }
            try
            {
                string criteria = "FillFinanceVoucherSummary";
                var dayBookData = _context.DayBookView.FromSqlRaw("Exec FinTransactionsSP @Criteria={0},@DateFrom={1},@DateUpto={2},@BranchID={3},@VoucherID={4}," +
                    "@UserID={5},@Detailed={6},@AutoEntry={7}",
                    criteria, dayBookDto.DateFrom, dayBookDto.DateUpto, dayBookDto.Branch.Id, dayBookDto.VoucherType.Id, dayBookDto.User.Id, dayBookDto.Detailed, false).ToList();
                return CommonResponse.Ok(dayBookData);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to fill DayBook");
                return CommonResponse.Error("Failed to fill DayBook");
            }
        }
    }
}
