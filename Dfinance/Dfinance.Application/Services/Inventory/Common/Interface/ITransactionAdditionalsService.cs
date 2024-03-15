﻿using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application
{
    public interface ITransactionAdditionalsService
    {
        CommonResponse SaveTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto);
        CommonResponse UpdateTransactionAdditional(FiTransactionAdditionalDto fiTransactionAdditionalDto);
        CommonResponse DeleteTransactionAdditional(int Id);
        CommonResponse FillTransactionAdditionals(int transactionId);
        CommonResponse GetTransPortationType();
        CommonResponse GetSalesArea();
        CommonResponse PopupVechicleNo();
        CommonResponse PopupDelivaryLocations(int salesManId);
    }
}
