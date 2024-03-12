using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application
{
    public class TransactionAdditionalsService : ITransactionAdditionalsService


    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public TransactionAdditionalsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        /// <summary>
        /// Fill Popup Area(from customer&Suppliyer)
        /// </summary>
        /// <returns></returns>
        //public CommonResponse PopTransactionAdditionals(int transactionId)
        //{
        //    try
        //    {
        //        var result = _context.FiTransactionAdditionals.Where(x => x.TransactionId == transactionId).Select(x => new {  Code = x.Code, Value = x.Name }).ToList();
        //        return CommonResponse.Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonResponse.Error(ex);
        //    }
        //}
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
        /////************* Fill TransactionAddtional By Id *******************/
        ////public CommonResponse FillTransactionalAdditionalById(int Id)
        ////{
        ////    try
        ////    {
        ////        var TId = _context.FiTransactionAdditionals.Where(i => i.TransactionId == Id).
        ////           Select(i => i.TransactionId).
        ////           SingleOrDefault();
        ////        if (TId == null)
        ////        {
        ////            return CommonResponse.NotFound("Area Not Found");
        ////        }
        ////        string criteria = "FillArea";
        ////        var result = _context.SpFillAreaMasterByIdG.FromSqlRaw($"EXEC SpArea @Criteria='{criteria}',@ID='{Id}'").ToList();
        ////        var res = result.Select(x => new SpFillAreaMasterByIdG
        ////        {
        ////            Id = x.Id,
        ////            Code = x.Code,
        ////            Name = x.Name,
        ////            IsGroup = x.IsGroup,
        ////            ParentId = x.ParentId,
        ////            ParentName = x.ParentName,
        ////            CreatedBranchId = x.CreatedBranchId,
        ////            CreatedBy = x.CreatedBy,
        ////            CreatedOn = x.CreatedOn,
        ////            Active = x.Active,
        ////            Note = x.Note
        ////        }).ToList();
        ////        return CommonResponse.Ok(res);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return CommonResponse.Error();
        ////    }
        ////}
        ///
        /****************** Save TransactionAdditional  *******************/
        public CommonResponse SaveTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto)
        {
            try
            {
                //int CreatedBranchId = _authService.GetBranchId().Value;
                //int CreatedBy = _authService.GetId().Value;
                //DateTime CreatedOn = DateTime.Now;
                string criteria = "InsertFiTransactionAdditionals";
                //SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                //{
                //    Direction = ParameterDirection.Output
                //};
                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@TypeID={2},@Name={3},@Address ={4}," +
                    "@Period={5},@LCNo={6},@InterestAmt={7},@DocumentNo={8},@DocumentDate={9},@EntryDate={10},@EntryNo={11},@BankAddress={12}," +
                    "@ExpiryDate={13},@SubmitDate={14},@PassNo={15},@ReferenceDate={16},@ReferenceNo={17},@RecommendNote={18},@AccountID={19}," +
                    "@AreaID={20},@PartyName={21},@Address1={22},@Address2={23}",
                    criteria,
                    fiTransactionAdditionalDto.TransactionId,
                    fiTransactionAdditionalDto.TransactionType.Id,
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
                    //newIdParameter
                    );
                //var newId = newIdParameter.Value;
                return CommonResponse.Created(fiTransactionAdditionalDto.TransactionId + ", " + fiTransactionAdditionalDto.PartyName + " Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /****************** Update TransactionAdditional  *******************/
        public CommonResponse UpdateTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto)
        {
            try
            {
                var TId = _context.FiTransactionAdditionals.Where(i => i.TransactionId == fiTransactionAdditionalDto.TransactionId).
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
                    fiTransactionAdditionalDto.TransactionId,
                    fiTransactionAdditionalDto.TransactionType.Id,
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

        /****************** Delete TransactionAdditional  *******************/
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

