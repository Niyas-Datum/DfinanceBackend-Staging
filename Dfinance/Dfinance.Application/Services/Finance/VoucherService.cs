using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace Dfinance.Application.Services.Finance
{
    public class VoucherService : IVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public VoucherService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        //voucher dropdown
        //used in DayBook filter
        //used in PageMenu
        public CommonResponse VoucherDropDown()
        {
            var vouchers = _context.DropDownViewName.FromSqlRaw("exec DropDownListSP @Criteria='FillVouchers'").ToList();
            return CommonResponse.Ok(vouchers);
        }
        //fill voucher popup-used in finance statements
        public CommonResponse VoucherPopup()
        {
            var result = _context.FiMaVouchers
                .Where(v => (v.ModuleType == 2 || v.ModuleType == 3) && v.Active == true)
                .Join(_context.MaPageMenus.Where(pm => pm.Active == true),
                      v => v.Id,
                      pm => pm.VoucherId,
                      (v, pm) => new { v.Id, v.Name })
                .Distinct()
                .ToList();
            return CommonResponse.Ok(result);
        }

        //called by VoucherController/FillVoucher
        //****************************FillFiMaVoucher****************************************
        public CommonResponse FillVoucher()
        {
            try
            {
                var result = _context.FillVoucherView.FromSqlRaw($"Exec FiMaVoucherSp @Criteria='FillFiMaVouchers'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        ////called by VoucherController/SaveVoucher
        ////****************************SaveVouchers****************************************
        //public CommonResponse SaveVoucher(VoucherDto voucherDto)
        //{
        //    try
        //    {
        //        var CreatedOn = DateTime.Now;
        //        var CreatedBy = _authService.GetId().Value;
        //        var CreatedBranchID = _authService.GetBranchId().Value;

        //        SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
        //        {
        //            Direction = ParameterDirection.Output
        //        };

        //        _context.Database.ExecuteSqlRaw("EXEC FiMaVoucherSp @Criteria='InsertFiMaVouchers',@NewID={0}  OUTPUT,@Name={1},@Alias={2},@PrimaryVoucherID={3},@Type={4},@Active={5},@Code={6},@DevCode={7}" +
        //            ",@CreatedBranchID={8},@CreatedBy={9},@CreatedOn={10},@DocumentType={11},@Numbering={12},@FinanceUpdate={13}, @RateUpdate={14}, @RowType={15}, @ApprovalRequired={16}, @WorkflowDays={17}, @ApprovalDays={18}, " +
        //            "@Nature={19}, @ModuleType={20}, @ReportPath={21} ",

        //               newIdparam,
        //               voucherDto.Name,
        //               voucherDto.Alias,
        //               voucherDto.PrimaryVoucherName,
        //               voucherDto.Type,
        //               voucherDto.Active,
        //               voucherDto.Code,
        //               voucherDto.DevCode,
        //               CreatedBranchID,
        //               CreatedBy,
        //               CreatedOn,
        //               voucherDto.DocumentTypeName,
        //               voucherDto.Numbering,
        //               voucherDto.FinanceUpdate,
        //               voucherDto.RateUpdate,
        //               voucherDto.RowType,
        //               voucherDto.ApprovalRequired,
        //               voucherDto.WorkflowDays,
        //               voucherDto.ApprovalDays,
        //               voucherDto.Nature,
        //               voucherDto.ModuleType,
        //               voucherDto.ReportPath

        //               );

        //        var NewID = newIdparam.Value;
        //        return CommonResponse.Ok("Saved");
        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonResponse.Error(ex);
        //    }

        //}

        //called by VoucherController/UpdateVoucher
        //****************************UpdateVoucher****************************************

        public CommonResponse UpdateVouchers(List<VoucherDto> voucherDtos)
        {
            try
            {
                string msg = null;

                var existingVouchers = _context.FiMaVouchers
                    .Where(v => voucherDtos.Select(dto => dto.Id).Contains(v.Id))
                    .ToList();

                if (existingVouchers.Count != voucherDtos.Count)
                {
                    msg = "Some Vouchers Not Found";
                    return CommonResponse.NotFound(msg);
                }

                var criteria = "UpdateFiMaVouchers";

                foreach (var voucherDto in voucherDtos)
                {
                    var voucherToUpdate = existingVouchers.FirstOrDefault(v => v.Id == voucherDto.Id);

                    if (voucherToUpdate != null)
                    {
                        _context.Database.ExecuteSqlRaw($"Exec FiMaVoucherSp @Criteria='{criteria}',@ID='{voucherDto.Id}',@Name='{voucherDto.Name}',@Alias='{voucherDto.Alias}',@PrimaryVoucherID='{voucherDto.PrimaryVoucherId}'," +
                            $"@Type='{voucherDto.Type}',@Active='{voucherDto.Active}',@Code='{voucherDto.Code}',@DevCode='{voucherDto.DevCode}',@DocumentType='{voucherDto.DocumentTypeId}'," +
                            $"@Numbering='{voucherDto.Numbering}',@FinanceUpdate='{voucherDto.FinanceUpdate}',@RateUpdate='{voucherDto.RateUpdate}',@RowType='{voucherDto.RowType}',@ApprovalRequired='{voucherDto.ApprovalRequired}'," +
                            $"@WorkflowDays='{voucherDto.WorkflowDays}',@ApprovalDays='{voucherDto.ApprovalDays}',@Nature='{voucherDto.Nature}',@ModuleType='{voucherDto.ModuleType}',@ReportPath='{voucherDto.ReportPath}'");
                    }
                }

                return CommonResponse.Ok("Vouchers Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }




        //called by VoucherController/Deletevoucher
        //****************************DeleteVoucher****************************************

        public CommonResponse Deletevoucher(int ID)
        {
            try
            {
                string msg = null;
                var desig = _context.FiMaVouchers.Where(i => i.Id == ID).SingleOrDefault();
                if (desig == null)
                {
                    msg = "Vouchers Not Found";
                    return CommonResponse.NotFound(msg);
                }

                msg = "Voucher " + desig + " is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw("EXEC FiMaVoucherSp @Criteria='DeleteFiMaVouchers',@ID={0}", ID);
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //called by VoucherController/FillPrimaryVoucherName
        //****************************FillPrimaryVoucherName****************************************
        public CommonResponse FillPrimaryVoucherName(int PrimaryVoucherId)
        {
            try
            {
                string msg = null;
                var desig = _context.FiMaVouchers.Any(i => i.PrimaryVoucherId == PrimaryVoucherId);

                if (!desig)
                {
                    msg = "Voucher Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "FillPrimaryVoucherName";
                var result = _context.NameView.FromSqlRaw($"Exec FiMaVoucherSp @Criteria='{criteria}',@PrimaryVoucherID='{PrimaryVoucherId}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //called by VoucherController/SaveVoucherNumbering
        //****************************SaveVoucherNumbering****************************************

        public CommonResponse SaveVoucherNumbering(NumberingDto numberingDto)
        {
            try
            {
                var numbering = new MaNumbering
                {
                    StartingNumber = numberingDto.StartingNumber,
                    MaximumDegits = numberingDto.MaximumDegits,
                    Prefillwithzero = true,
                    Renewedby = 0,
                    Prefix = 0,
                    PrefixValue = null,
                    Suffix = 0,
                    SuffixValue = null,
                    Editable = false
                };

                _context.MaNumbering.Add(numbering);
                _context.SaveChanges();

                return CommonResponse.Ok(numbering);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //called by VoucherController/UpdateVoucherNumbering
        //****************************UpdateVoucherNumbering****************************************

        public CommonResponse UpdateVoucherNumbering(NumberingDto numberingDto, int Id)
        {
            try
            {
                var nData = _context.MaNumbering.Where(x => x.Id == Id).FirstOrDefault();
                if (nData == null)
                    return CommonResponse.NotFound("Id not found!");
                else
                {
                    nData.StartingNumber = numberingDto.StartingNumber;
                    nData.MaximumDegits = numberingDto.MaximumDegits;
                    _context.Update(nData);
                    _context.SaveChanges();
                    return CommonResponse.Ok(nData);
                }

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }

        }

        //called by VoucherController/DeleteVoucherNumbering
        //****************************DeleteVoucherNumbering****************************************
        public CommonResponse DeleteVoucherNumbering(int Id)
        {
            try
            {
                var isInUse = _context.FiMaVouchers.Any(f => f.Numbering == Id);

                if (isInUse)
                {
                    return CommonResponse.Error("Cannot delete because it is referenced in Vouchers.");
                }
                var delete = _context.MaNumbering.FirstOrDefault(x => x.Id == Id);
                if (delete == null)
                {
                    return CommonResponse.NotFound("Id not found!");
                }
                else
                {
                    _context.Remove(delete);
                    _context.SaveChanges();

                    return CommonResponse.Ok(delete);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateUpto"></param>
        /// <param name="branchid"></param>
        /// <param name="Detailed"></param>
        /// <param name="transactionno"></param>
        /// <param name="customersupplier"></param>
        /// <param name="item"></param>
        /// <param name="voucher"></param>
        /// <param name="PreVoucherID"></param>
        /// <returns></returns>
        public CommonResponse GetVoucherHistory(DateTime DateFrom, DateTime DateUpto, int branchid,
           bool Detailed, string transactionno, int? customersupplier,
           int? item, int? voucher, int? PreVoucherID)
        {
            try
            {
                object result = null;
                string Criteria = "VoucherDetails";
                var query = new StringBuilder();
                query.Append("Exec VoucherReportSP ");
                query.AppendFormat("@Criteria = {0}, ", Criteria);
                query.AppendFormat("@DateFrom = '{0}', ", DateFrom.ToString("yyyy-MM-dd"));
                query.AppendFormat("@DateUpto = '{0}', ", DateUpto.ToString("yyyy-MM-dd"));
                query.AppendFormat("@BranchID = {0}, ", branchid);
                query.AppendFormat("@Detailed = {0}, ", Detailed);
                if (transactionno != null)
                {
                    query.AppendFormat(", @TransactionNo = {0}", transactionno);
                }
                if (customersupplier != 0 && customersupplier != null)
                {
                    query.AppendFormat(", @AccountID = {0}", customersupplier);
                }
                if (item != 0 && item != null)
                {
                    query.AppendFormat(", @ItemID = {0}", item);
                }
                if (voucher != 0 && voucher != null)
                {
                    query.AppendFormat(", @VTypeID = {0}", voucher);
                }
                if (PreVoucherID != 0 && PreVoucherID != null)
                {
                    query.AppendFormat(", @PreVoucherID = {0}", PreVoucherID);
                }
                if (query.ToString().EndsWith(", "))
                {
                    query.Length -= 2;
                }
                result = _context.VoucherHistoryView.FromSqlRaw(query.ToString()).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse VoucherPopupEreturn()
        {
            try
            {
                var result = (from V in _context.FiMaVouchers
                              join F in _context.FiTransaction on V.Id equals F.VoucherId
                              where (V.FinanceUpdate.HasValue && V.FinanceUpdate.Value == true) && V.Active == true
                              select new
                              {
                                  V.Id,
                                  V.Name
                              })
                       .Distinct()
                       .ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
    }
}

