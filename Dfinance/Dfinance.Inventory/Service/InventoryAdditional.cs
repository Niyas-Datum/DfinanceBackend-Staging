using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Dfinance.Inventory.Service
{
    public class InventoryAdditional:IInventoryAdditional
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        
    public InventoryAdditional(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
    {
        _context = context;
        _authService = authService;
        _environment = hostEnvironment;

       
    }
    /// <summary>
    /// PopupVechicleNo in Additionaldetails
    /// </summary>
    /// <returns></returns>
    public CommonResponse PopupVechicleNo()
    {
        try
        {
            var result = _context.MaVehicles.Where(x => !string.IsNullOrEmpty(x.CostCenterId.ToString()) && x.ActiveFlag == 1).Select(x => new { VehicleNo = x.RegistrationNo, Name = x.Name, ID = x.Id }).ToList();
            return CommonResponse.Ok(result);
        }
        catch (Exception ex)
        {
            return CommonResponse.Error(ex);
        }
    }
    /// <summary>
    /// Fill Popup Delivary Location(from customer&Suppliyer)
    /// </summary>
    /// <returns></returns>
    public CommonResponse PopupDelivaryLocations(int salesManId)
    {
        try
        {
            var result = _context.DeliveryDetails.Where(x => x.PartyId == (_context.Parties.Where(p => p.AccountId == salesManId).Select(p => p.Id).FirstOrDefault())).Select(x => new { Location = x.LocationName, ProjectName = x.ProjectName, ContactPerson = x.ContactPerson, ContactNo = x.ContactNo, Address = x.Address, Party = x.Party.Name, ID = x.Id }).ToList();
            return CommonResponse.Ok(result);
        }
        catch (Exception ex)
        {
            return CommonResponse.Error(ex);
        }
    }
    /************* Fill all TransactionAdditionals  *******************/
    public CommonResponse FillTransactionAdditionals(int transactionId)
    {
        try
        {
            string criteria = "FillFiTransactionAdditionals";
            var result = _context.SpGetTransactionAdditionals.FromSqlRaw($"EXEC VoucherAdditionalsSP @Criteria='{criteria}',@TransactionID='{transactionId}'").ToList();

            return CommonResponse.Ok(result);
        }
        catch (Exception ex)
        {
            return CommonResponse.Error();
        }
    }
    /////************* Fill TransPortation Type By Criteria *******************/
    public CommonResponse GetTransPortationType()
    {
        try
        {

            string criteria = "FillMaMisc";
            string key = "Transportation Mode";
            var result = _context.SpFillAreaMasterByIdG.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}',@StrParam='{key}'").ToList();
            return CommonResponse.Ok(result);
        }
        catch (Exception ex)
        {
            return CommonResponse.Error();
        }
    }
    /// <summary>
    ///  /////************* Fill SalesArea By Criteria *******************/
    public CommonResponse GetSalesArea()
    {
        try
        {

            string criteria = "FillArea";
            var result = _context.SpFillAreaMasterByIdG.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
            return CommonResponse.Ok(result);
        }
        catch (Exception ex)
        {
            return CommonResponse.Error();
        }
    }
        /// </summary>
        /// <param name="fiTransactionAdditionalDto"></param>
        /// <returns></returns>
        /****************** Save TransactionAdditional  *******************/
        public CommonResponse SaveTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto, int TransId)
        {
            string criteria = "InsertFiTransactionAdditionals";

            _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@TypeID={2},@Name={3},@Address ={4}," +
                "@Period={5},@LCNo={6},@InterestAmt={7},@DocumentNo={8},@DocumentDate={9},@EntryDate={10},@EntryNo={11},@BankAddress={12}," +
                "@ExpiryDate={13},@SubmitDate={14},@PassNo={15},@ReferenceDate={16},@ReferenceNo={17},@RecommendNote={18},@AccountID={19}," +
                "@AreaID={20},@PartyName={21},@Address1={22},@Address2={23}",
                criteria,
                TransId,
                fiTransactionAdditionalDto.TransPortationType.Id == 0 ? null : fiTransactionAdditionalDto.TransPortationType.Id,
                fiTransactionAdditionalDto.PartyNameandAddress,
                fiTransactionAdditionalDto.TermsOfDelivery,
                fiTransactionAdditionalDto.CreditPeriod,
                fiTransactionAdditionalDto.MobileNo,
                fiTransactionAdditionalDto.StaffIncentives,
                fiTransactionAdditionalDto.DespatchNo,
                fiTransactionAdditionalDto.DespatchDate,
                fiTransactionAdditionalDto.PartyDate,
                fiTransactionAdditionalDto.PartyInvoiceNo,
                fiTransactionAdditionalDto.Attention,
                fiTransactionAdditionalDto.ExpiryDate,
                fiTransactionAdditionalDto.DespatchDate,
                fiTransactionAdditionalDto.DeliveryDate,
                fiTransactionAdditionalDto.OrderDate,
                fiTransactionAdditionalDto.OrderNo,
                fiTransactionAdditionalDto.DelivaryLocation.Name,
                fiTransactionAdditionalDto.SalesMan.Id == 0 ? null : fiTransactionAdditionalDto.SalesMan.Id,
                fiTransactionAdditionalDto.SalesArea.Id == 0 ? null : fiTransactionAdditionalDto.SalesArea.Id,
                fiTransactionAdditionalDto.PartyName,
                fiTransactionAdditionalDto.AddressLine1,
                fiTransactionAdditionalDto.AddressLine2

                );

            return CommonResponse.Ok();
        }
        /// <summary>
        /// UpdateTransactionAdditional
        /// </summary>
        /// <param name="fiTransactionAdditionalDto"></param>
        /// <param name="TransId"></param>
        /// <returns></returns>
        /****************** Update TransactionAdditional  *******************/
        public CommonResponse UpdateTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto, int TransId)
        {
            try
            {
                var TId = _context.FiTransactionAdditionals.Where(i => i.TransactionId == TransId).
                   Select(i => i.TransactionId).
                   SingleOrDefault();
                if (TId == null)
                {
                    return CommonResponse.NotFound("TransactionAdditional Not Found");
                }

                string criteria = "UpdateFiTransactionAdditionals";
                var result = _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@TypeID={2},@Name={3},@Address ={4}," +
                    "@Period={5},@LCNo={6},@InterestAmt={7},@DocumentNo={8},@DocumentDate={9},@EntryDate={10},@EntryNo={11},@BankAddress={12}," +
                    "@ExpiryDate={13},@SubmitDate={14},@PassNo={15},@ReferenceDate={16},@ReferenceNo={17},@RecommendNote={18},@AccountID={19}," +
                    "@AreaID={20},@PartyName={21},@Address1={22},@Address2={23}",
                criteria,
                    TransId,
                    fiTransactionAdditionalDto.TransPortationType.Id,
                    fiTransactionAdditionalDto.PartyNameandAddress,
                    fiTransactionAdditionalDto.TermsOfDelivery,
                    fiTransactionAdditionalDto.CreditPeriod,
                    fiTransactionAdditionalDto.MobileNo,
                    fiTransactionAdditionalDto.StaffIncentives,
                    fiTransactionAdditionalDto.DespatchNo,
                    fiTransactionAdditionalDto.DespatchDate,
                    fiTransactionAdditionalDto.PartyDate,
                    fiTransactionAdditionalDto.PartyInvoiceNo,
                    fiTransactionAdditionalDto.Attention,
                    fiTransactionAdditionalDto.ExpiryDate,
                    fiTransactionAdditionalDto.DespatchDate,
                    fiTransactionAdditionalDto.DeliveryDate,
                    fiTransactionAdditionalDto.OrderDate,
                    fiTransactionAdditionalDto.OrderNo,
                    fiTransactionAdditionalDto.DelivaryLocation.Name,
                    fiTransactionAdditionalDto.SalesMan.Id,
                    fiTransactionAdditionalDto.SalesArea.Id,
                    fiTransactionAdditionalDto.PartyName,
                    fiTransactionAdditionalDto.AddressLine1,
                    fiTransactionAdditionalDto.AddressLine2
                    );
                return CommonResponse.Ok("TransactionAdditional Updated Successfully");

            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /// <summary>
        /// deleteTransactionAdditional
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse DeleteTransactionAdditional(int Id)
        {
            try
            {
                string msg = null;
                var name = _context.FiTransactionAdditionals.Where(i => i.TransactionId == Id).
                    Select(i => i.Name).
                    SingleOrDefault();
                if (name == null)
                {
                    msg = "TransactionAdditional Not Found";
                }
                else
                {
                    string criteria = "DeleteFiTransactionAdditionals";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC VoucherAdditionalsSP @Criteria='{criteria}',@TransactionID='{Id}'");
                    msg = name + " Is Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
    }
    }
