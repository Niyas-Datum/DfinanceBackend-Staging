using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace Dfinance.Inventory.Service
{
    public class TransactionFactory:ITransactionFactory
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        public TransactionFactory(DFCoreContext context,IAuthService authService,IHostEnvironment hostEnvironment) 
        { 
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
        }

        public CommonResponse GetSalesman()
        {
            try
            {
                var result = _context.FiMaAccounts
                .Where(a => a.AccountCategory == 3 && a.Active)
                .Select(a => new
                {
                    Code = a.Alias,
                    Name = a.Name,
                    ID = a.Id
                })
                .ToList();

                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="transactionDto"></param>
       /// <returns></returns>
        public CommonResponse SaveTransaction(PurchaseDto transactionDto)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                int BranchId = _authService.GetBranchId().Value;
                int CreatedBy = _authService.GetId().Value;
                int? serialno = null;
                bool isPostDatedValue = false;
                int VoucherId = 17;
                int? refPageTypeIdValue = null;
                int? refPageTableIdValue = null;
                int? finYearIdValue = null;
                string instrumentTypeValue = null;
                string instrumentNoValue = null;
                DateTime? instrumentDateValue = null;
                string Description = null;
                string instrumentBankValue = null;
                int addedByValue = CreatedBy;
                int approvedByValue = 0;
                string approvalStatusValue = "A";
                DateTime? approvedDateValue = null;
                string approveNoteValue = null;
                string actionValue = null;
                int statusIdValue = 806;
                bool isAutoEntryValue = true;
                bool postedValue = true;
                bool cancelledValue = false;
                bool activeValue = true;
                int pageIdValue = 15;
                int? refTransIdValue = null;
                string machineNameValue = _environment.EnvironmentName;
                string criteria = "InsertTransactions";
                SqlParameter newIdItem = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
            "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @SerialNo={4}, @TransactionNo={5}, " +
            "@IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, @RefPageTableID={10}, " +
            "@ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, @InstrumentNo={15}, " +
            "@InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, @ApprovedBy={20}, " +
            " @ApprovedDate={21}, @ApprovalStatus={22}, @ApproveNote={23}, @Action={24}, @StatusID={25}, " +
            "@IsAutoEntry={26}, @Posted={27}, @Active={28}, @Cancelled={29}, @AccountID={30}, @Description={31}, " +
            "@RefTransID={32}, @CostCentreID={33}, @PageID={34}, @MachineName={35}, @NewID OUTPUT",
            criteria, transactionDto.Date, dateTime, VoucherId, serialno, transactionDto.VoucherNo,
            isPostDatedValue, transactionDto.Currency, transactionDto.ExchangeRate, refPageTypeIdValue, refPageTableIdValue,
            transactionDto.Reference.Id, BranchId, finYearIdValue, instrumentTypeValue, instrumentNoValue,
            instrumentDateValue, instrumentBankValue, transactionDto.Description, addedByValue, approvedByValue,
            approvedDateValue, approvalStatusValue, approveNoteValue, actionValue, statusIdValue,
            isAutoEntryValue, postedValue, activeValue, cancelledValue, transactionDto.Supplier, Description,
            refTransIdValue, transactionDto.Project, pageIdValue, machineNameValue, newIdItem);

                return CommonResponse.Created("Created Successfully");
            }

            catch
            {
                return CommonResponse.Error("Created UnSuccessfully");
            }
        }

    }
}
