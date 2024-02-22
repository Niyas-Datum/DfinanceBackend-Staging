using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.Finance
{
    public class FinanceYearService : IFinanceYearService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public FinanceYearService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        //*****************************FillAllFinanceYear**************************************************
        //called by FinanceYearController/FillAllFinanceYear
        public CommonResponse FillAllFinanceYear()
        {
            try
            {
                string criteria = "FillFinYearMaster";
                var result = _context.FinanceYearView.FromSqlRaw($"Exec spFinanceYear @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //*****************************FillFinanceYearById**************************************************

        //called by FinanceYearController/FillFinanceYearById
        public CommonResponse FillFinanceYearById(int Id)
        {
            try
            {
                string criteria = "FillFinYearWithID";
                var result = _context.FinanceYearViewByID.FromSqlRaw($"Exec spFinanceYear @Criteria='{criteria}',@FinYearID={Id}").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }

        }
    
        //*****************************SaveFinanceYear**************************************************
        //called by FinanceYearController/SaveFinanceYear
        public CommonResponse SaveFinanceYear(FinanceYearDto financeYearDto)
        {
            try
            {
                int CreatedBranchId = _authService.GetBranchId().Value;
                int createdby = _authService.GetId().Value;
                string criteria = "InserttblMaFinYear";


                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("Exec spFinanceYear @Criteria={0},@FinYearCode={1},@StartDate={2},@EndDate={3},@LockTillDate={4},@Status={5},@CreatedBy={6},@BranchID={7},@NewID={8} OUTPUT", criteria, financeYearDto.FinanceYear,financeYearDto.Startdate,financeYearDto.Enddate,financeYearDto.LockTillDate,financeYearDto.Status,createdby, CreatedBranchId ,newId);
                var CCNewId = newId.Value;
                return CommonResponse.Created("FinanceYear " + financeYearDto.FinanceYear + " is Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //*****************************UpdateFinanceYear**************************************************
        //called by FinanceYearController/UpdateFinanceYear
        public CommonResponse UpdateFinanceYear(FinanceYearDto financeYearDto, int Id)
        {
            try {
                string msg = null;
                var finyearid = _context.TblMaFinYear.Find(Id);
                if (finyearid == null)
                {
                    msg = "FinanceYear Not Found";
                    return CommonResponse.NotFound(msg);
                }
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "UpdatetblMaFinYear";
                var finaceyear = _context.Database.ExecuteSqlRaw("Exec spFinanceYear @Criteria={0},@FinYearCode={1},@StartDate={2},@EndDate={3},@LockTillDate={4},@Status={5},@CreatedBy={6},@BranchID={7},@CreatedOn={8},@FinYearID={9} ", criteria, financeYearDto.FinanceYear, financeYearDto.Startdate, financeYearDto.Enddate, financeYearDto.LockTillDate, financeYearDto.Status, CreatedBy, CreatedBranchId, CreatedOn,Id);
                return CommonResponse.Created(finaceyear);
            } catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
                        }
        }
        //*****************************DeleteFinanceYear**************************************************
        //called by FinanceYearController/DeleteFinanceYear
        public CommonResponse DeleteFinanceYear(int Id)
        {
            string msg = null;
            var finyear = _context.TblMaFinYear.Where(i => i.FinYearId == Id).
                Select(i => i.FinYearCode).
                SingleOrDefault();
            if (finyear == null)
            {
                msg = "FinanceYear Not Found";
                return CommonResponse.NotFound(msg);
            }

            int criteria = 3;
            msg = "FinanceYear " + finyear + " is Deleted Successfully";
            var result = _context.Database.ExecuteSqlRaw($"EXEC spFinanceYear @Mode='{criteria}', @FinYearID={Id}");
            return CommonResponse.Ok(msg);
        }
    }
}
