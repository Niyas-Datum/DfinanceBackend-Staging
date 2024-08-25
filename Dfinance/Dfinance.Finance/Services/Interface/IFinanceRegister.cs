using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services.Interface
{
    public interface IFinanceRegister
    {
        public CommonResponse FillFinanceRegister(DateTime DateFrom, DateTime DateUpto, int BranchID, int? BasicVTypeID = null, int? VTypeID = null, bool Detailed = false, bool Inventory = false, bool Columnar = false, bool GroupItem = false, string Criteria = "", int? AccountID = null, int? PaymentTypeID = null, int? ItemID = null, int? CounterID = null, string PartyInvNo = "", string BatchNo = "", int? UserID = null, int? StaffID = null, int? AreaID = null, int? pageId = null);
       
        public CommonResponse FillAccountPopup();
        public CommonResponse FillVoucherType(int primaryVoucherId);
        public CommonResponse FillBasicType();
        }
    }

