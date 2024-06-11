using AutoMapper;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.Finance
{
    public class FinanceYearService : IFinanceYearService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<FinanceYearService> _logger;
        public FinanceYearService(DFCoreContext context, IAuthService authService, IMapper mapper,ILogger<FinanceYearService> logger)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }

        //*****************************FillAllFinanceYear**************************************************
        //called by FinanceYearController/FillAllFinanceYear
        public CommonResponse FillAllFinanceYear()
        {
            try
            {
                string criteria = "FillFinYearMaster";
                var result = _context.FinanceYearView.FromSqlRaw($"Exec spFinanceYear @Criteria='{criteria}'").ToList();
                _logger.LogInformation("Success FillFinanceYear");
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogInformation("Success Finance Year FillById");
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                SetCurrentStatus(financeYearDto);

                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("Exec spFinanceYear @Criteria={0},@FinYearCode={1},@StartDate={2},@EndDate={3},@LockTillDate={4},@Status={5},@CreatedBy={6},@BranchID={7},@NewID={8} OUTPUT", criteria, financeYearDto.FinanceYear, financeYearDto.StartDate, financeYearDto.EndDate, financeYearDto.LockTillDate, financeYearDto.Status, createdby, CreatedBranchId, newId);
                var CCNewId = newId.Value;
                _logger.LogInformation("FinanceYear is Created Successfully");
                return CommonResponse.Created("FinanceYear " + financeYearDto.FinanceYear + " is Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

        private void SetCurrentStatus(FinanceYearDto financeYearDto)
        {
            var currentFinYear = _context.TblMaFinYear.Where(f => f.Status == "R").FirstOrDefault();

            if (financeYearDto.Status == "R")
            {               
                if (currentFinYear != null)
                {
                    if (currentFinYear.EndDate <= financeYearDto.StartDate)
                    {
                        currentFinYear.Status = "C";
                    }
                    else
                    {
                        currentFinYear.Status = "O";
                    }
                    var currentFinYeartbl = _mapper.Map<TblMaFinYear, FinanceYearDto>(currentFinYear);
                    UpdateFinanceYear(currentFinYeartbl, currentFinYear.FinYearId);
                }
            }
        }

        //*****************************UpdateFinanceYear**************************************************
        //called by FinanceYearController/UpdateFinanceYear
        public CommonResponse UpdateFinanceYear(FinanceYearDto financeYearDto, int Id)
        {
            try
            {
                string msg = null;
                var finyearid = _context.TblMaFinYear.Find(Id);
                if (finyearid == null)
                {
                    msg = "FinanceYear Not Found";
                    _logger.LogInformation(msg);
                    return CommonResponse.Error(msg);
                }
                if (finyearid.Status == "R")
                {
                    msg = "Current Finance Year Status can't Updated.Please Insert First";
                    _logger.LogInformation(msg);
                    return CommonResponse.Error(msg);
                }
                SetCurrentStatus(financeYearDto);
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "UpdatetblMaFinYear";
                var finaceyear = _context.Database.ExecuteSqlRaw("Exec spFinanceYear @Criteria={0},@FinYearCode={1},@StartDate={2},@EndDate={3},@LockTillDate={4},@Status={5},@CreatedBy={6},@BranchID={7},@CreatedOn={8},@FinYearID={9} ", criteria, financeYearDto.FinanceYear, financeYearDto.StartDate, financeYearDto.EndDate, financeYearDto.LockTillDate, financeYearDto.Status, CreatedBy, CreatedBranchId, CreatedOn, Id);
                _logger.LogInformation("Update Successfully");
                return CommonResponse.Created(finaceyear);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        //*****************************DeleteFinanceYear**************************************************
        //called by FinanceYearController/DeleteFinanceYear
        public CommonResponse DeleteFinanceYear(int Id)
        {
            try
                {
                string msg = null;
                var finyear = _context.TblMaFinYear.Where(i => i.FinYearId == Id).
                    Select(i => i.Status).
                    SingleOrDefault();
                if (finyear == null)
                {
                    msg = "FinanceYear Not Found";
                    _logger.LogInformation(msg);
                    return CommonResponse.Error(msg);
                }
                else if (finyear != "R")
                {
                    int criteria = 3;
                    msg = "FinanceYear " + finyear + " is Deleted Successfully";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spFinanceYear @Mode='{criteria}', @FinYearID={Id}");
                }
                else
                {
                    msg = "Can't delete Current Finance Year";
                }
                _logger.LogInformation(msg);
                return CommonResponse.Ok(msg);
                }
            
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
            
        }
    }
}
