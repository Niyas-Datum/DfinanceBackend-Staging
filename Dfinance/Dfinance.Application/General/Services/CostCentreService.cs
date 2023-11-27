using Dfinance.Application.Dto.General;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Dfinance.Application.General.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using Dfinance.Application.Services.Interface.General;


namespace Dfinance.Application.General.Services
{
    public class CostCentreService:ICostCentreService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public CostCentreService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public CommonResponse FillCostCentre()
        {
            try
            {
                string criteria = "FillCostCentreMaster";
                var result=_context.SpFillCostCentreG.FromSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}'").ToList();               
                return CommonResponse.Ok(result);
            }
            catch(Exception ex) 
            { 
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillCostCentreById(int Id)
        {
            try
            {
                string criteria = "FillCostCentre";
                var result= _context.SpFillCostCentreByIdG.FromSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@ID='{Id}'").ToList();               
                return CommonResponse.Ok(result);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveCostCentre(CostCentreDto costCentreDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
               
                SqlParameter newIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                string criteria = "Insert";
                _context.Database.ExecuteSqlRaw("EXEC CostCentreSP @Criteria={0},@Code={1},@Description={2},@InActive={3},"+
                    "@PType={4},@Type={5},@SerialNo={6},@RegNo={7},@SupplierID={8},@Status={9},@Remarks={10},@Rate={11},"+
                    "@SDate={12},@Make={13},@MYear={14},@EDate={15},@ContractValue={16},@InvoicedAmt={17},@ClientID={18},"+
                    "@StaffID={19},@IsPaid={20},@StaffID1={21},@Site={22},@IsGroup={23},@CostCategoryID={24},@ParentID={25},"+
                    "@CreatedOn={26},@CreatedBy={27},@CreatedBranchID={28},@NewID={29} OUTPUT",
                    criteria,
                    costCentreDto.Code,
                    costCentreDto.Name,
                    costCentreDto.Active,
                    costCentreDto.Nature.Key,
                    null,
                    costCentreDto.SerialNo,
                    costCentreDto.RegNo,
                    costCentreDto.Consultancy,
                    costCentreDto.Status.Value,
                    costCentreDto.Remarks,
                    costCentreDto.Rate,
                    costCentreDto.StartDate,
                    costCentreDto.Make,
                    costCentreDto.MakeYear,
                    costCentreDto.EndDate,
                    costCentreDto.ContractValue,
                    costCentreDto.InvoiceValue,
                    costCentreDto.Client,
                    costCentreDto.Engineer,
                    null,
                    costCentreDto.Foreman,
                    costCentreDto.Site,
                    costCentreDto.IsGroup,
                    costCentreDto.Category.Id,
                    costCentreDto.CreateUnder.Id,
                    CreatedOn,
                    CreatedBy,
                    CreatedBranchId,
                    newIdParam
                    );
               
                return CommonResponse.Created("Cost Center "+costCentreDto.Name+" Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse UpdateCostCentre(CostCentreDto costCentreDto,int Id)
        {
            try
            {
                if (Id == 0)
                    return CommonResponse.NotFound("CostCentre Not Found");

                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "Update";
                var result = _context.Database.ExecuteSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@ID='{Id}',@Code='{costCentreDto.Code}',@Description='{costCentreDto.Name}',@InActive='{costCentreDto.Active}',@PType='{costCentreDto.Nature}',@Type='{null}',@SerialNo='{costCentreDto.SerialNo}',@RegNo='{costCentreDto.RegNo}',@SupplierID='{costCentreDto.Consultancy}',@Status='{costCentreDto.Status.Value}',@Remarks='{costCentreDto.Remarks}',@Rate='{costCentreDto.Rate}',@SDate='{costCentreDto.StartDate}',@Make='{costCentreDto.Make}',@MYear='{costCentreDto.MakeYear}',@EDate='{costCentreDto.EndDate}',@ContractValue='{costCentreDto.ContractValue}',@InvoicedAmt='{costCentreDto.InvoiceValue}',@ClientID='{costCentreDto.Client}',@StaffID='{costCentreDto.Engineer}',@IsPaid='{null}',@StaffID1='{costCentreDto.Foreman}',@Site='{costCentreDto.Site}',@IsGroup='{costCentreDto.IsGroup}',@CostCategoryID='{costCentreDto.Category.Id}',@ParentID='{costCentreDto.CreateUnder.Id}',@CreatedOn='{CreatedOn}',@CreatedBy='{CreatedBy}',@CreatedBranchID='{CreatedBranchId}'");
                return CommonResponse.Ok("CostCentre Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse DeleteCostCentre(int Id)
        {
            try
            {
                if (Id == 0)
                    return CommonResponse.NotFound();

                string msg = null;
                var costcentre = _context.CostCentre.Where(i => i.Id == Id).
                    Select(i=>i.Description).
                    SingleOrDefault();
                if (costcentre == null)
                {
                    msg = "This CostCentre is Not Found";
                }
                else
                {
                    string criteria = "Delete";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC CostCentreSP @Criteria='{criteria}',@ID='{Id}'");
                   msg= costcentre + " Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        /************  CostCentre DropDown   ************/
            public CommonResponse FillCostCentreDropDown()
            {
                try
                {
                    string criteria = "FillCostCenterGroup";
                    var data = _context.SpDropDownCommon.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
                    return CommonResponse.Ok(data);
                }
                catch (Exception ex)
                {
                    return CommonResponse.Error(ex);
                }
            }

        /**************** For Client Pop Up *********************/
        public CommonResponse FillPopUp(string Description)
        {
            try
            {
                int Branch = _authService.GetBranchId().Value;
                var result = (from A in _context.FiMaAccounts
                                       join AC in _context.FiMaAccountCategory on A.AccountCategory equals AC.Id
                                       join BA in _context.FiMaBranchAccounts on A.Id equals BA.AccountId
                                       where AC.Description == Description && BA.BranchId == Branch
                                       select new FillPopupView
                                       {
                                           ID = A.Id,
                                           Name = A.Name,
                                           Code = A.Alias
                                       }).ToList();                               

                return CommonResponse.Ok(result);
            }
            catch(Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
       

        }
    }
